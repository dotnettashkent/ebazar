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
		public async virtual Task<TableResponse<ProductResultView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var product = from s in dbContext.Products select s;
			if (!String.IsNullOrEmpty(options.Search))
			{
				product = product.Where(s =>
						 s.NameUz != null && s.NameUz.Contains(options.Search)
						 || s.NameRu.Contains(options.Search)
				);
			}

			Sorting(ref product, options);


			var count = await product.AsNoTracking().CountAsync();
			var items = await product.AsNoTracking().Paginate(options).ToListAsync();

			return new TableResponse<ProductResultView>() { Items = items.MapToViewListResult(), TotalItems = count };
		}
		public async virtual Task<ProductResultView> GetById(long Id, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var product = await dbContext.Products
				.FirstOrDefaultAsync(x => x.Id == Id);
			
			return product == null ? throw new ValidationException("ProductEntity Not Found") : product.MapToView();
		}

        public async virtual Task<ProductView> Get(long Id, CancellationToken cancellationToken = default)
        {
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var product = await dbContext.Products
                .FirstOrDefaultAsync(x => x.Id == Id);

            return product == null ? throw new ValidationException("ProductEntity Not Found") : product.MapToView2();
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

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            ProductEntity entity = new ProductEntity();
            Reattach(entity, command.Entity, dbContext);

            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();
        }



        public async virtual Task Delete(DeleteProductCommand command, CancellationToken cancellationToken = default)
		{
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var entity = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == command.Id);
            
			if (entity == null) 
				throw new ValidationException("ProductEntity Not Found");
            dbContext.Remove(entity);
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
            var entity = await dbContext.Products
				.FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

            if (entity == null) 
				throw new ValidationException("ProductEntity Not Found");

            Reattach(entity, command.Entity, dbContext);

            await dbContext.SaveChangesAsync();
        }

		#endregion

		#region Helpers
		//[ComputeMethod]
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

		private ProductEntity Reattach(ProductEntity entity, ProductResultView view, AppDbContext dbContext)
		{
			ProductMapper.From2(view, entity);
			return entity;

		}

		private void Sorting(ref IQueryable<ProductEntity> offering, TableOptions options) => offering = options.SortLabel switch
		{
            "NameUz" => offering.Ordering(options, o => o.NameUz),
            "NameRu" => offering.Ordering(options, o => o.NameRu),
            "BrandName" => offering.Ordering(options, o => o.BrandName),
            "DescriptionUz" => offering.Ordering(options, o => o.DescriptionUz),
            "DescriptionRu" => offering.Ordering(options, o => o.DescriptionRu),
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
