using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Stl.CommandR.Configuration;
using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;

namespace Shared.Features
{
	public interface IOrderServices : IComputeService
	{
        //[ComputeMethod]
        Task<TableResponse<OrderView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);

        //[ComputeMethod]
        Task<OrderResponse> Get(long Id, CancellationToken cancellationToken = default);

        [CommandHandler]
        Task Create(CreateOrderCommand command, CancellationToken cancellationToken = default);

        [CommandHandler]
        Task Delete(DeleteOrderCommand command, CancellationToken cancellationToken = default);

        [CommandHandler]
        Task Update(UpdateOrderCommand command, CancellationToken cancellationToken = default);
        
        //[CommandHandler]
        //Task UpdateItem(UpdateItemOrderCommand command, CancellationToken cancellationToken = default);

        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
