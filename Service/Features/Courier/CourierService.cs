using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Shared.Infrastructures;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructures.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Service.Features.Courier
{
	public class CourierService : ICourierService
	{
		#region Initialize

		private readonly DbHub<AppDbContext> dbHub;

		public CourierService(DbHub<AppDbContext> dbHub)
		{
			this.dbHub = dbHub;
		}
		#endregion
		#region Queries
		public async virtual Task<TableResponse<CourierView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var couriers = from s in dbContext.Couriers select s;

			if (!String.IsNullOrEmpty(options.Search))
			{
				var search = options.Search.ToLower();
				couriers = couriers.Where(s =>
						 s.FirstName != null && s.FirstName.ToLower().Contains(search)
						|| s.LastName != null && s.LastName.ToLower().Contains(search)
						|| s.MiddleName != null && s.MiddleName.ToLower().Contains(search)
						|| s.PhoneNumber != null && s.PhoneNumber.ToLower().Contains(search)
						|| s.PassportPINFL != null && s.PassportPINFL.Contains(search)
				);
			}

			Sorting(ref couriers, options);

			var count = await couriers.AsNoTracking().CountAsync();
			var items = await couriers.AsNoTracking().Paginate(options).ToListAsync();
			return new TableResponse<CourierView>() { Items = items.MapToViewList(), TotalItems = count };
		}

		public async virtual Task<CourierView> GetById(long id, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var user = await dbContext.Couriers
			.FirstOrDefaultAsync(x => x.Id == id);

			return user == null ? throw new ValidationException("Courier was not found") : user.MapToView();
		}
		#endregion
		#region Mutations
		public async virtual Task Create(CreateCourierCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}

			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			CourierEntity courier = new CourierEntity();
			Reattach(courier, command.Entity, dbContext);

			dbContext.Update(courier);
			await dbContext.SaveChangesAsync(cancellationToken);
		}
		public async virtual Task Delete(DeleteCourierCommand command, CancellationToken cancellationToken = default)
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
		public async virtual Task Update(UpdateCourierCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var user = await dbContext.Couriers
			.FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

			if (user == null) throw new ValidationException("CourierEntity Not Found");

			Reattach(user, command.Entity, dbContext);

			await dbContext.SaveChangesAsync(cancellationToken);
		}

		#endregion
		#region Helpers

		//[ComputeMethod]
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
		private void Reattach(CourierEntity user, CourierView userView, AppDbContext dbContext)
		{
			CourierMapper.From(userView, user);
		}

		private void Sorting(ref IQueryable<CourierEntity> unit, TableOptions options) => unit = options.SortLabel switch
		{
			"FirstName" => unit.Ordering(options, o => o.FirstName),
			"LastName" => unit.Ordering(options, o => o.LastName),
			"MiddleName" => unit.Ordering(options, o => o.MiddleName),
			"PhoneNumber" => unit.Ordering(options, o => o.PhoneNumber),
			"CreatedAt" => unit.Ordering(options, o => o.CreatedAt),
			_ => unit.OrderBy(o => o.CreatedAt),
		};

		#endregion
	}
}
