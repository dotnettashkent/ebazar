using Microsoft.AspNetCore.Http;
using Stl.Fusion;

namespace Shared.Features
{
    public interface IFileService : IComputeService
    {
        public Task<Tuple<int, string>> SaveImage(IFormFile imageFile);
        public Task<bool> DeleteImage(string imageFileName);
    }
}
