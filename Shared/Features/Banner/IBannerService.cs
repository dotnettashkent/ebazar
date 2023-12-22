using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features.Banner
{
    public interface IBannerService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<BannerView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
		//[ComputeMethod]
		Task<List<BannerView>> Get(long Id, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Create(CreateBannerCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Update(UpdateBannerCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Delete(DeleteBannerCommand command, CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
