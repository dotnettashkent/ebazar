using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Shared.Infrastructures;
using Stl.CommandR.Configuration;
using Shared.Infrastructures.Extensions;

namespace Shared.Features
{
    public interface IProductService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<ProductResultView>> GetAll(TableOptions options , CancellationToken cancellationToken = default);
        //[ComputeMethod]
        Task<ProductView> Get(long Id, CancellationToken cancellationToken = default);

        [ComputeMethod]
        Task<ProductResultView> GetById(long Id, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Create(CreateProductCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Delete(DeleteProductCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Update(UpdateProductCommand command , CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
