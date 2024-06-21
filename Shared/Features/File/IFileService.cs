using Microsoft.AspNetCore.Http;
using Stl.Fusion;

namespace Shared.Features
{
    public interface IFileService : IComputeService
    {
        Task<bool> DeleteVideoAsync(string subpath);
        Task<string> UplaodVideoAsync(IFormFile file);

        public Task<bool> DeleteOneImage(string imageFileName, string token);
    }
}
