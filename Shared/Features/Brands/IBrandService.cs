using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Shared.Infrastructures;
using Stl.CommandR.Configuration;
using Shared.Infrastructures.Extensions;

namespace Shared.Features
{
    public interface IBrandService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<BrandView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
        //[ComputeMethod]
        Task<BrandView> Get(long Id, string token, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Create(CreateBrandCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Update(UpdateBrandCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Delete(DeleteBrandCommand command, CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
