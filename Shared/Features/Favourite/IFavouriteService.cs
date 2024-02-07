using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;


namespace Shared.Features
{
    public interface IFavouriteService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<ProductResultView>> GetAll(long UserId, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Create(CreateFavouriteCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Delete(DeleteFavouriteCommand command, CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
