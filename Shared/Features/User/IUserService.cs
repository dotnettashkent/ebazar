using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Shared.Infrastructures;
using Stl.CommandR.Configuration;
using Shared.Infrastructures.Extensions;

namespace Shared.Features
{
	public interface IUserService : IComputeService
	{
		//[ComputeMethod]
		Task<TableResponse<UserView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
		//[ComputeMethod]
		Task<UserView> GetById(long id, CancellationToken cancellationToken = default);

		Task<UserResultView> Get(long Id, CancellationToken cancellationToken = default);

		Task<UserView> Login(string email, string password);

		Task<UserView> GetByToken(string token);

		[CommandHandler]
		Task Create(CreateUserCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Delete(DeleteUserCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Update(UpdateUserCommand command, CancellationToken cancellationToken = default);
		Task<Unit> Invalidate() { return TaskExt.UnitTask; }
	}
}
