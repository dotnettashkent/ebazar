using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared;
public interface ILocaleService : IComputeService
{
    //[ComputeMethod]
    Task<TableResponse<LocaleView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
    //[ComputeMethod]
    Task<LocaleView> Get(string Code, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Create(CreateLocaleCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Update(UpdateLocaleCommand command, CancellationToken cancellationToken = default);
    [CommandHandler]
    Task Delete(DeleteLocaleCommand command, CancellationToken cancellationToken = default);
    Task<Unit> Invalidate() { return TaskExt.UnitTask; }
}
