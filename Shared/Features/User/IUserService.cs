using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;
using System.Threading.Tasks;

namespace Shared.Features
{
    public interface IUserService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<UserView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
        Task<TableResponse<OrderResultView>> GetUserOrdersByProcessAsync(TableOptions options, CancellationToken cancellationToken = default);
        //[ComputeMethod]
        Task<UserView> GetById(long id, CancellationToken cancellationToken = default);
        Task<UserResultView> Get(long Id, CancellationToken cancellationToken = default);
        Task<string> Login(string phoneNumber, string password);
        Task<string> AdminLogin(string phoneNumber, string password);
        Task<UserView> GetByToken(string token, CancellationToken cancellationToken);
        
        [CommandHandler]
        Task<bool> Create(CreateUserCommand command, CancellationToken cancellationToken = default);
        
        [CommandHandler]
        Task Delete(DeleteUserCommand command, CancellationToken cancellationToken = default);
        
        [CommandHandler]
        Task Update(UpdateUserCommand command, CancellationToken cancellationToken = default);
        
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
