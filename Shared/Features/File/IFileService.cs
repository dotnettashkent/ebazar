using Stl.Fusion;
using Microsoft.AspNetCore.Http;

namespace Shared.Features
{
    public interface IFileService : IComputeService
    {
        public Task<Tuple<int, string>> SaveImage(IFormFile imageFile);
        public Task<bool> DeleteImage(string imageFileName);
    }
}
