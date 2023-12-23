using Service.Data;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.Fusion.EntityFramework;
using System.Reactive;

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
		public async virtual Task<TableResponse<FavouriteView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var users = from s in dbContext.Favorites select s;

			if (!String.IsNullOrEmpty(options.Search))
			{
				var search = options.Search.ToLower();
				users = users.Where(s =>
						 s.FirstName != null && s.FirstName.ToLower().Contains(search)
						|| s.LastName != null && s.LastName.ToLower().Contains(search)
						|| s.FatherName != null && s.FatherName.ToLower().Contains(search)
						|| s.Username != null && s.Username.ToLower().Contains(search)
						|| s.PhoneNumber != null && s.PhoneNumber.Contains(search)
				);
			}

			Sorting(ref users, options);

			var count = await users.AsNoTracking().CountAsync();
			var items = await users.AsNoTracking().Paginate(options).ToListAsync();
			return new TableResponse<UserView>() { Items = items.MapToViewList(), TotalItems = count };
		}

		public Task<FavouriteView> GetById(long id, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Mutations
		public Task Create(CreateFavouriteCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}

		public Task Delete(DeleteFavouriteCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}
		public Task Update(UpdateFavouriteCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
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
