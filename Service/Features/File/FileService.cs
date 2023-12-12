using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;

namespace Service.Features.File
{
    public class FileService : IFileService
    {
        public Task Create(CreateFileCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Delete(DeleteFileCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<FileView> Get(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<TableResponse<FileView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Update(UpdateFileCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
