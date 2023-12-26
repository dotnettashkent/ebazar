using Stl.Async;
using Stl.Fusion;
using Service.Data;
using System.Reactive;
using Shared.Features;
using Shared.Infrastructures;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructures.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Service.Features
{
	public class FavouriteService : IFavouriteService
	{
		#region Initialize
		private readonly DbHub<AppDbContext> dbHub;

		public FavouriteService(DbHub<AppDbContext> dbHub)
		{
			this.dbHub = dbHub;
		}
		#endregion

		#region Queries
		public async virtual Task<TableResponse<FavouriteView>> GetAll(long UserId, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var users = from s in dbContext.Favorites select s;
			users = users.Where(x => x.UserId == UserId);

			var count = await users.AsNoTracking().CountAsync();
			var items = await users.AsNoTracking().ToListAsync();
			return new TableResponse<FavouriteView>() { Items = items.MapToViewList(), TotalItems = count };
		}

		#endregion

		#region Mutations
		public async virtual Task Create(CreateFavouriteCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}

			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			FavouriteEntity category = new FavouriteEntity();
			Reattach(category, command.Entity, dbContext);

			dbContext.Update(category);
			await dbContext.SaveChangesAsync();
		}

		public async virtual Task Delete(DeleteFavouriteCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var brand = await dbContext.Favorites
			.FirstOrDefaultAsync(x => x.Id == command.Id);
			if (brand == null) throw new ValidationException("FavouriteEntity Not Found");
			dbContext.Remove(brand);
			await dbContext.SaveChangesAsync();
		}
		#endregion

		#region Helpers
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

		private void Reattach(FavouriteEntity favourite, FavouriteView favouriteView, AppDbContext dbContext)
		{
			FavouriteMapper.From(favouriteView, favourite);
		}
		#endregion

	}
}
