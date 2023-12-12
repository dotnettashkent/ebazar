using Shared.Infrastructures;
using Stl.Async;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features.Product
{
    public interface IProductService : IComputeService
    {
        [ComputeMethod]
        Task<List<ProductView>> GetAll(TableOptions options , CancellationToken cancellationToken = default);
        [ComputeMethod]
        Task<ProductView> GetById(long id, CancellationToken cancellationToken = default);
        Task Create(CreateProductCommand command, CancellationToken cancellationToken = default);
        Task Delete(DeleteProductCommand command, CancellationToken cancellationToken = default);
        Task Update(UpdateProductCommand command , CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
