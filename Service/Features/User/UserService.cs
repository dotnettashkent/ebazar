using Microsoft.EntityFrameworkCore;
using Service.Data;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.Fusion;
using Stl.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reactive;
using System.Security.Principal;

namespace Service.Features.User
{
	public class UserService : IUserService
	{
		#region Initialize

		private readonly DbHub<AppDbContext> dbHub;

		public UserService(DbHub<AppDbContext> dbHub)
		{
			this.dbHub = dbHub;
		}
		#endregion

		#region Queries
		public async virtual Task<TableResponse<UserView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var users = from s in dbContext.UsersEntities select s;

			if (!String.IsNullOrEmpty(options.Search))
			{
				var search = options.Search.ToLower();
				users = users.Where(s =>
						 s.FirstName != null && s.FirstName.ToLower().Contains(search)
						|| s.LastName != null && s.LastName.ToLower().Contains(search)
						|| s.MiddleName != null && s.MiddleName.ToLower().Contains(search)
						|| s.PhoneNumber != null && s.PhoneNumber.ToLower().Contains(search)
						|| s.Email != null && s.Email.Contains(search)
				);
			}

			Sorting(ref users, options);

			var count = await users.AsNoTracking().CountAsync();
			var items = await users.AsNoTracking().Paginate(options).ToListAsync();
			return new TableResponse<UserView>() { Items = items.MapToViewList(), TotalItems = count };
		}

		public async virtual Task<UserView> GetById(long id, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var user = await dbContext.UsersEntities
			.FirstOrDefaultAsync(x => x.Id == id);

			return user == null ? throw new ValidationException("User was not found") : user.MapToView();
		}
		#endregion

		#region Mutations
		public async virtual Task Create(CreateUserCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}

			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			UserEntity goldnumber = new UserEntity();
			Reattach(goldnumber, command.Entity, dbContext);

			dbContext.Update(goldnumber);
			await dbContext.SaveChangesAsync(cancellationToken);
		}
		public async virtual Task Delete(DeleteUserCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var user = await dbContext.UsersEntities
			.FirstOrDefaultAsync(x => x.Id == command.Id);
			if (user == null) throw new ValidationException("UserEntity Not Found");
			dbContext.Remove(user);
			await dbContext.SaveChangesAsync(cancellationToken);
		}
		public async virtual Task Update(UpdateUserCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var user = await dbContext.UsersEntities
			.FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

			if (user == null) throw new ValidationException("UserEntity Not Found");

			Reattach(user, command.Entity, dbContext);

			await dbContext.SaveChangesAsync(cancellationToken);
		}

		#endregion

		#region Helpers

		//[ComputeMethod]
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
		private void Reattach(UserEntity user, UserView userView, AppDbContext dbContext)
		{
			UserMapper.From(userView, user);
		}

		private void Sorting(ref IQueryable<UserEntity> unit, TableOptions options) => unit = options.SortLabel switch
		{
			"FirstName" => unit.Ordering(options, o => o.FirstName),
			"LastName" => unit.Ordering(options, o => o.LastName),
			"MiddleName" => unit.Ordering(options, o => o.MiddleName),
			"Email" => unit.Ordering(options, o => o.Email),
			"PhoneNumber" => unit.Ordering(options, o => o.PhoneNumber),
			"CreatedAt" => unit.Ordering(options, o => o.CreatedAt),
			_ => unit.OrderBy(o => o.CreatedAt),
		};

		#endregion







	}
}
