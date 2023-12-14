﻿using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features
{
	public interface IUserService : IComputeService
	{
		//[ComputeMethod]
		Task<TableResponse<UserView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
		//[ComputeMethod]
		Task<UserView> GetById(long id, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Create(CreateUserCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Delete(DeleteUserCommand command, CancellationToken cancellationToken = default);
		[CommandHandler]
		Task Update(UpdateUserCommand command, CancellationToken cancellationToken = default);
		Task<Unit> Invalidate() { return TaskExt.UnitTask; }
	}
}
