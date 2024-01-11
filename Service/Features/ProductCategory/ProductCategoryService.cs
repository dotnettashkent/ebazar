/*using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Shared.Infrastructures;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Service.Features.ProductCategory;
using Shared.Infrastructures.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Service.Features
{
	public class ProductCategoryService : IProductCategoryService
	{
		#region Initialize
		private readonly DbHub<AppDbContext> dbHub;
		public ProductCategoryService(DbHub<AppDbContext> dbHub)
		{
			this.dbHub = dbHub;
		}
		#endregion

		#region Queries
		

		[ComputeMethod]
		public virtual async Task<TableResponse<ProductCategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var category = from s in dbContext.ProductCategories select s;

			if (!String.IsNullOrEmpty(options.Search))
			{
				category = category.Where(s =>
						 s.Name.Contains(options.Search)
				);
			}

			Sorting(ref category, options);

			var count = await category.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
			var items = await category.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
			return new TableResponse<ProductCategoryView>() { Items = items.MapToViewList(), TotalItems = count };
		}

		[ComputeMethod]
		public async virtual Task<ProductCategoryView> Get(long Id, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var category = await dbContext.ProductCategories
				.FirstOrDefaultAsync(x => x.Id == Id);

			return category == null ? throw new ValidationException("ProductSubCategoryEntity Not Found") : category.MapToView();
		}
		#endregion
		#region Mutations
		public async virtual Task Create(CreateProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}

			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			ProductCategoryEntity category = new ProductCategoryEntity();
			Reattach(category, command.Entity, dbContext);

			dbContext.Update(category);
			await dbContext.SaveChangesAsync();
		}

		public async virtual Task Delete(DeleteProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var category = await dbContext.ProductCategories
				.FirstOrDefaultAsync(x => x.Id == command.Id);
			if (category == null) throw new ValidationException("ProductCategoryEntity Not Found");
			dbContext.Remove(category);
			await dbContext.SaveChangesAsync();
		}

		public async virtual Task Update(UpdateProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var category = await dbContext.ProductCategories
				.FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

			if (category == null) throw new ValidationException("ProductCategoryEntity Not Found");

			Reattach(category, command.Entity, dbContext);

			await dbContext.SaveChangesAsync();
		}

		
		#endregion
		#region Helpers

		[ComputeMethod]
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

		private ProductCategoryEntity Reattach(ProductCategoryEntity entity, ProductCategoryView view, AppDbContext dbContext)
		{
			ProductCategoryMapper.From(view, entity);
			return entity;

		}

		private void Sorting(ref IQueryable<ProductCategoryEntity> tag, TableOptions options) => tag = options.SortLabel switch
		{
			"Id" => tag.Ordering(options, o => o.Id),
			"Name" => tag.Ordering(options, o => o.Name),
			_ => tag.OrderBy(o => o.Id),

		};
		#endregion
	}
}
*/