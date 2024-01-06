
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;

namespace Service.Features
{
	public class ProductSubCategoryService : IProductSubCategoryService
	{
		public Task Create(CreateProductSubCategoryCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task Delete(DeleteProductSubCategoryCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<List<ProductCategoryView>> Get(long Id, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task<TableResponse<ProductCategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task Update(UpdateProductSubCategoryCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
	}
}
