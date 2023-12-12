using Shared.Infrastructures;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features
{
    public interface IFileService
    {
        //[ComputeMethod]
        Task<List<FileView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
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
