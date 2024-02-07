using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features
{
    public interface IBrandService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<BrandView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
        //[ComputeMethod]
        Task<BrandView> Get(long Id, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Create(CreateBrandCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Update(UpdateBrandCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Delete(DeleteBrandCommand command, CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
