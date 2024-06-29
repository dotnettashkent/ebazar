using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features
{
    public interface ICartService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<ProductResultView>> GetAll(string token, CancellationToken cancellationToken = default);

        //[ComputeMethod]
        //Task<ProductResultView> Get(long Id, CancellationToken cancellationToken= default);

        [CommandHandler]
        Task Create(CreateCartCommand command, CancellationToken cancellationToken = default);
        
        [CommandHandler]
        Task UpdateProductQuantityAsync(UpdateCartCommand command, CancellationToken cancellationToken = default);

        [CommandHandler]
        Task Delete(DeleteCartCommand command, CancellationToken cancellationToken = default);

        //[CommandHandler]
        Task RemoveAll(string token, CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
