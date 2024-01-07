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
			product = product.Include(p => p.Category);
			product = product.Include(p => p.SubCategory);

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
			}

			if (command.Entity.Count != 3)
				throw new Exception("Product must be in 3 languages!");

			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);

			try
			{
				var maxId = await dbContext.Products.Select(x => x.Id).MaxAsync();
				maxId++;

				foreach (var item in command.Entity)
				{
					ProductEntity category = new ProductEntity();
					category = Reattach(category, item, dbContext);
					category.Id = maxId;
					dbContext.Products.Add(category);
				}
				await dbContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public async virtual Task Delete(DeleteProductCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
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
				foreach (var entity in command.Entity)
				{
					_ = await GetFor(entity.Id, entity.Locale, cancellationToken);
				}
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

		[ComputeMethod]
		public async virtual Task<ProductEntity?> GetFor(long id, string locale, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);

			var tag = await dbContext.Products
				.FirstOrDefaultAsync(x => x.Id == id && x.Locale == locale);

			return tag is null ? null : tag;
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
