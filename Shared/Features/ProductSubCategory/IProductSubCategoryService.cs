using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features
{
	public interface IProductSubCategoryService : IComputeService
	{
		[ComputeMethod]
		Task<TableResponse<ProductSubCategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
		[ComputeMethod]
		Task<List<ProductSubCategoryView>> Get(long Id, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Create(CreateProductSubCategoryCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Delete(DeleteProductSubCategoryCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Update(UpdateProductSubCategoryCommand command, CancellationToken cancellationToken = default);
		Task<Unit> Invalidate() { return TaskExt.UnitTask; }
	}
}
