
namespace Shared.Features
{
    public interface IAuthService
    {
        ValueTask<string> GenerateTokenAsync(string login, string password);
    }
}
