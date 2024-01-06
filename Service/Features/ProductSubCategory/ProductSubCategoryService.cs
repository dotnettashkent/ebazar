
using Service.Data;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Fusion.EntityFramework;

namespace Service.Features
{
	public class ProductSubCategoryService : IProductSubCategoryService
	{
		#region Initialize
		private readonly DbHub<AppDbContext> _dbHub;
		//ProductSubCategoryEntity
		public ProductSubCategoryService(DbHub<AppDbContext> dbHub)
		{
			_dbHub = dbHub;
		}
		#endregion
		public Task Create(CreateProductSubCategoryCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task Delete(DeleteProductSubCategoryCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<List<ProductSubCategoryView>> Get(long Id, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TableResponse<ProductSubCategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task Update(UpdateProductSubCategoryCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}
