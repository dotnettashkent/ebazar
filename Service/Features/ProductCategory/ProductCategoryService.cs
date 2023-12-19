using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Shared.Features;
using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Stl.Fusion.EntityFramework;
using Service.Data;
using Microsoft.EntityFrameworkCore;
using Service.Features.ProductCategory;

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
		public async virtual Task<TableResponse<ProductCategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();

			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var categories = from s in dbContext.ProductCategories select s;

			if (!String.IsNullOrEmpty(options.Search))
			{
				categories = categories.Where(s =>
						 s.MainLinkEn.Contains(options.Search)
						|| s.MainLinkRu.Contains(options.Search)
						|| s.MainNameUz.Contains(options.Search)
				);
			}

			var count = await categories.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
			var items = await categories.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
			return new TableResponse<ProductCategoryView>() { Items = items.MapToViewList(), TotalItems = count };
		}

		public Task<ProductCategoryView> GetById(long id, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Mutations
		public Task Create(CreateProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task Delete(DeleteProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task Update(UpdateProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Helpers

		[ComputeMethod]
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
		#endregion
	}
}
