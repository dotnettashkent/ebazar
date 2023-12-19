using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features
{
	public interface IFavouriteService
	{
		[ComputeMethod]
		Task<TableResponse<FavouriteView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
		[ComputeMethod]
		Task<FavouriteView> GetById(long id, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Create(CreateProductCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Delete(DeleteProductCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Update(UpdateProductCommand command, CancellationToken cancellationToken = default);
		Task<Unit> Invalidate() { return TaskExt.UnitTask; }
	}
}
