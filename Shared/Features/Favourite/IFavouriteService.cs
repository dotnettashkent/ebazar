using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Shared.Infrastructures;
using Stl.CommandR.Configuration;
using Shared.Infrastructures.Extensions;

namespace Shared.Features
{
	public interface IFavouriteService
	{
		[ComputeMethod]
		Task<TableResponse<FavouriteView>> GetAll(long UserId, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Create(CreateFavouriteCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Delete(DeleteFavouriteCommand command, CancellationToken cancellationToken = default);
		Task<Unit> Invalidate() { return TaskExt.UnitTask; }
	}
}
