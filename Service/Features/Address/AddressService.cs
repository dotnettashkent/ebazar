using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.Features.Address;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.Fusion;
using Stl.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Reactive;

namespace Service.Features
{
	public class AddressService : IAddressService
	{
		#region Initialize
		private readonly DbHub<AppDbContext> dbHub;

		public AddressService(DbHub<AppDbContext> dbHub)
		{
			this.dbHub = dbHub;
		}
		#endregion
		#region Queries
		public async virtual Task<TableResponse<AddressView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var address = from s in dbContext.Addresses select s;

			if (!String.IsNullOrEmpty(options.Search))
			{
				address = address.Where(s =>
						 s.Region.Contains(options.Search)
						 || s.District.Contains(options.Search)
						 || s.Street.Contains(options.Search)
				);
			}

			Sorting(ref address, options);

			var count = await address.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
			var items = await address.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
			return new TableResponse<AddressView>() { Items = items.MapToViewList(), TotalItems = count };
		}
		public async virtual Task<AddressView> Get(long id, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var address = await dbContext.Addresses
			.FirstOrDefaultAsync(x => x.Id == id);

			return address == null ? throw new ValidationException("AddressEntity Not Found") : address.MapToView();
		}

		#endregion

		#region Mutations
		public async virtual Task Create(CreateAddressCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}

			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			AddressEntity address = new AddressEntity();
			Reattach(address, command.Entity, dbContext);

			dbContext.Update(address);
			await dbContext.SaveChangesAsync();
		}

		public async virtual Task Delete(DeleteAddressCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var address = await dbContext.Addresses
			.FirstOrDefaultAsync(x => x.Id == command.Id);
			if (address == null) throw new ValidationException("AddressEntity Not Found");
			dbContext.Remove(address);
			await dbContext.SaveChangesAsync();
		}


		public async virtual Task Update(UpdateAddressCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var address = await dbContext.Addresses
			.FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

			if (address == null) throw new ValidationException("AddressEntity Not Found");

			Reattach(address, command.Entity, dbContext);

			await dbContext.SaveChangesAsync();
		}
		#endregion

		#region Helpers

		//[ComputeMethod]
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
		private void Reattach(AddressEntity entity, AddressView view, AppDbContext dbContext)
		{
			AddressMapper.From(view, entity);
		}

		private void Sorting(ref IQueryable<AddressEntity> unit, TableOptions options) => unit = options.SortLabel switch
		{
			"Id" => unit.Ordering(options, o => o.Id),
			"Region" => unit.Ordering(options, o => o.Region),
			"District" => unit.Ordering(options, o => o.District),
			"Street" => unit.Ordering(options, o => o.Street),
			"HomeNumber" => unit.Ordering(options, o => o.HomeNumber),
			_ => unit.OrderBy(o => o.Id),
		};
		#endregion
	}
}
