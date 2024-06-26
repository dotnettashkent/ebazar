using Microsoft.AspNetCore.Http;
using Stl.Fusion;

namespace Shared.Features
{
    public interface IFileService : IComputeService
    {
        Task<bool> DeleteMediaAsync(string subpath);
        Task<string> UploadMediaAsync(IFormFile file);

        public Task<bool> DeleteOneImage(string imageFileName, string token);
    }
}
