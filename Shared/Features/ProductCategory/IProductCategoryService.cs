using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features
{
	public interface IProductCategoryService : IComputeService
	{
		[ComputeMethod]
		Task<TableResponse<ProductCategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
		[ComputeMethod]
		Task<ProductCategoryView> GetById(long id, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Create(CreateProductCategoryCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Delete(DeleteProductCategoryCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Update(UpdateProductCategoryCommand command, CancellationToken cancellationToken = default);
		Task<Unit> Invalidate() { return TaskExt.UnitTask; }
	}
}
