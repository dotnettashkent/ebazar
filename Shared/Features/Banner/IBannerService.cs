using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Shared.Infrastructures;
using Stl.CommandR.Configuration;
using Shared.Infrastructures.Extensions;

namespace Shared.Features
{
    public interface IBannerService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<BannerView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
		//[ComputeMethod]
		Task<BannerView> Get(long Id, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Create(CreateBannerCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Update(UpdateBannerCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Delete(DeleteBannerCommand command, CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
