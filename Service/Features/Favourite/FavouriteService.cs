using Stl.Async;
using Stl.Fusion;
using Service.Data;
using System.Reactive;
using Shared.Features;
using Stl.Fusion.EntityFramework;
using Shared.Infrastructures.Extensions;

namespace Service.Features
{
	public class FavouriteService : IFavouriteService
	{
		#region Initialize
		private readonly DbHub<AppDbContext> dbHub;
		private readonly IProductService productService;
        public FavouriteService(DbHub<AppDbContext> dbHub, IProductService productService)
        {
            this.dbHub = dbHub;
            this.productService = productService;
        }
        #endregion

        #region Queries
        public async virtual Task<TableResponse<ProductResultView>> GetAll(long UserId, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var favourites = from s in dbContext.Favorites select s;
			favourites = favourites.Where(x => x.UserId == UserId);
			
			var listProductId = new List<long>();
			foreach (var item in favourites)
			{
                listProductId.AddRange(item.Products);
			}
			var productList = new List<ProductResultView>();
			foreach (var item in listProductId)
			{
				var productGetResult = await productService.GetById(item, cancellationToken);
				productList.Add(productGetResult);
			}
			var count = productList.Count();
			return new TableResponse<ProductResultView>() { Items = productList, TotalItems = count};
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
			var exists = dbContext.Favorites.FirstOrDefault(x => x.UserId == command.Entity.UserId);
			if (exists != null)
			{
				exists.Products.AddRange(command.Entity.Products);
			}
			else 
			{
				FavouriteEntity category = new FavouriteEntity();
				Reattach(category, command.Entity, dbContext);
				dbContext.Update(category);
			}

			await dbContext.SaveChangesAsync();
		}

		public async virtual Task Delete(DeleteFavouriteCommand command, CancellationToken cancellationToken = default)
		{
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
			var products = new List<long>();
			products.AddRange(command.Entity.Products);
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var exists = dbContext.Favorites.FirstOrDefault(x => x.UserId == command.Entity.UserId);
			if (exists != null)
			{
				foreach (var item in products)
				{
					exists.Products.Remove(item);
				}
			}

            await dbContext.SaveChangesAsync(cancellationToken);
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
