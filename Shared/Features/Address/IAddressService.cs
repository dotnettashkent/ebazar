using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features
{
    public interface IAddressService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<AddressView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
        //[ComputeMethod]
        Task<AddressView> Get(long id, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Create(CreateAddressCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Update(UpdateAddressCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Delete(DeleteAddressCommand command, CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
