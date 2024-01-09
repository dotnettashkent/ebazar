using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Shared.Infrastructures;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructures.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Service.Features
{
	public class ProductService : IProductService
	{
		#region Initialize
		private readonly DbHub<AppDbContext> dbHub;

		public ProductService(DbHub<AppDbContext> dbHub)
		{
			this.dbHub = dbHub;
		}
		#endregion

		#region Queries
		public async virtual Task<TableResponse<ProductView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var product = from s in dbContext.Products select s;
			if (!String.IsNullOrEmpty(options.Search))
			{
				product = product.Where(s =>
						 s.Name != null && s.Name.Contains(options.Search)
				);
			}

			Sorting(ref product, options);


			var count = await product.AsNoTracking().CountAsync();
			var items = await product.AsNoTracking().Paginate(options).ToListAsync();

			return new TableResponse<ProductView>() { Items = items.MapToViewList(), TotalItems = count };
		}
		public async virtual Task<ProductView> Get(long Id, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var product = await dbContext.Products
				.FirstOrDefaultAsync(x => x.Id == Id);
			
			return product == null ? throw new ValidationException("ProductEntity Not Found") : product.MapToView();
		}
        #endregion

        #region Mutations
        public async virtual Task Create(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }

            if (command.Entity.Count != 3)
                throw new Exception("Product must be 3 language!");

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            long maxId = 0;

            // Check if there are any records in the Products table
            if (await dbContext.Products.AnyAsync())
            {
                // If there are records, get the maximum Id
                maxId = await dbContext.Products.Select(x => x.Id).MaxAsync();
            }

            // Increment maxId by 1
            maxId++;

            try
            {
				var all = new List<ProductEntity>();
                for (int i = 0; i < command.Entity.Count; i++)
                {
                    ProductEntity section = new ProductEntity();

                    // Create a new instance of ProductEntity for each item
                    section = Reattach(section, command.Entity[i], dbContext);

                    // Set the Id and Locale for each entity
                    section.Id = maxId; // Set your desired Id here
					all.Add(section);
                    // Attach the entity to the DbContext
                }
				dbContext.AddRange(all);
                // Save changes to the database
                await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }



        public async virtual Task Delete(DeleteProductCommand command, CancellationToken cancellationToken = default)
		{
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var tag = await dbContext.Products
			.Where(x => x.Id == command.Id).ToListAsync();

			if (tag == null)
				throw new ValidationException("ProductEntity Not Found");
			dbContext.Remove(tag);
			await dbContext.SaveChangesAsync();
		}
		public async virtual Task Update(UpdateProductCommand command, CancellationToken cancellationToken = default)
		{
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);

			foreach (var entityToUpdate in command.Entity)
			{
				var tag = await dbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entityToUpdate.Id && x.Locale == entityToUpdate.Locale);

				if (tag == null)
				{
					throw new ValidationException($"ProductEntity with Id {entityToUpdate.Id} not found");
				}

				Reattach(tag, entityToUpdate, dbContext);

				dbContext.Update(tag);
			}

			await dbContext.SaveChangesAsync();
		}

		#endregion

		#region Helpers
		//[ComputeMethod]
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

		private ProductEntity Reattach(ProductEntity entity, ProductView view, AppDbContext dbContext)
		{
			ProductMapper.From(view, entity);
			return entity;

		}

		private void Sorting(ref IQueryable<ProductEntity> offering, TableOptions options) => offering = options.SortLabel switch
		{
			"Name" => offering.Ordering(options, o => o.Name),
			"BrandName" => offering.Ordering(options, o => o.BrandName),
			"Description" => offering.Ordering(options, o => o.Description),
			"Price" => offering.Ordering(options, o => o.Price),
			"DiscountPrice" => offering.Ordering(options, o => o.DiscountPrice),
			"Tag" => offering.Ordering(options, o => o.Tag),
			"CreatedAt" => offering.Ordering(options, o => o.CreatedAt),
			"IsActive" => offering.Ordering(options, o => o.IsActive),

			"IsPopular" => offering.Ordering(options, o => o.IsPopular),
			"IsHoliday" => offering.Ordering(options, o => o.IsHoliday),
			"IsBigSale" => offering.Ordering(options, o => o.IsBigSale),
			_ => offering.OrderByDescending(o => o.Id)
		};
		#endregion

	}
}
