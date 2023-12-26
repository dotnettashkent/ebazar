using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Shared.Infrastructures;
using Stl.CommandR.Configuration;
using Shared.Infrastructures.Extensions;

namespace Shared.Features
{
    public interface IFileService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<FileView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
        //[ComputeMethod]
        Task<FileView> Get(long Id, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Create(CreateFileCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Update(UpdateFileCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Delete(DeleteFileCommand command, CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
