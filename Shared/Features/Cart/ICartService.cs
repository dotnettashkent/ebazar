using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Stl.CommandR.Configuration;
using Shared.Infrastructures.Extensions;

namespace Shared.Features
{
	public interface ICartService : IComputeService
	{
		//[ComputeMethod]
		Task<TableResponse<ProductResultView>> GetAll(long UserId, CancellationToken cancellationToken = default);

		//[ComputeMethod]
		//Task<ProductResultView> Get(long Id, CancellationToken cancellationToken= default);

		[CommandHandler]
		Task Create(CreateCartCommand command, CancellationToken cancellationToken = default);
		
		[CommandHandler]
		Task Delete(DeleteCartCommand command, CancellationToken cancellationToken = default);
		
		Task<Unit> Invalidate() { return TaskExt.UnitTask; }
	}
}
