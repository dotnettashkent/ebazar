using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Shared.Infrastructures;
using Stl.CommandR.Configuration;
using Shared.Infrastructures.Extensions;

namespace Shared.Features
{
    public interface ICourierService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<CourierView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
        //[ComputeMethod]
        Task<CourierView> GetById(long id, string token, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Create(CreateCourierCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Delete(DeleteCourierCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Update(UpdateCourierCommand command, CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
