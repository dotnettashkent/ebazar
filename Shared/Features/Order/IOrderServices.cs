/*using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Shared.Infrastructures;
using Stl.CommandR.Configuration;
using Shared.Infrastructures.Extensions;

namespace Shared.Features
{
	public interface IOrderServices
	{
		[ComputeMethod]
		Task<TableResponse<OrderView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
		[ComputeMethod]
		Task<OrderView> GetById(long id, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Create(CreateOrderCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Delete(DeleteOrderCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Update(UpdateOrderCommand command, CancellationToken cancellationToken = default);
		Task<Unit> Invalidate() { return TaskExt.UnitTask; }
	}
}
*/