using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Service.Features.Cart;
using Stl.Fusion.EntityFramework;
using Shared.Infrastructures.Extensions;

namespace Service.Features
{
	public class CartService : ICartService
	{
		#region Initialize
		private readonly DbHub<AppDbContext> dbHub;
		private readonly IProductService productService;
        public CartService(DbHub<AppDbContext> dbHub, IProductService productService)
        {
            this.dbHub = dbHub;
            this.productService = productService;
        }
        #endregion

        #region Queries
        public async virtual Task<TableResponse<ProductView>> GetAll(long UserId, CancellationToken cancellationToken = default)
		{
            await Invalidate();
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var favourites = from s in dbContext.Carts select s;
            favourites = favourites.Where(x => x.UserId == UserId);

            var listProductId = new List<long>();
            foreach (var item in favourites)
            {
                listProductId.AddRange(item.ProductIds);
            }
            var productList = new List<ProductView>();
            foreach (var item in listProductId)
            {
                var productGetResult = productService.Get(item, cancellationToken).Result;
                productList.Add(productGetResult);
            }
            var count = productList.Count();
            return new TableResponse<ProductView>() { Items = productList, TotalItems = count };
        }

		#endregion

		#region Mutations
		public async virtual Task Create(CreateCartCommand command, CancellationToken cancellationToken = default)
		{
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var exists = dbContext.Carts.FirstOrDefault(x => x.UserId == command.Entity.UserId);
            if (exists != null)
            {
                exists.ProductIds.AddRange(command.Entity.ProductIds);
            }
            else
            {
                CartEntity category = new CartEntity();
                Reattach(category, command.Entity, dbContext);
                dbContext.Update(category);
            }

            await dbContext.SaveChangesAsync();
        }


		public async virtual Task Delete(DeleteCartCommand command, CancellationToken cancellationToken = default)
		{
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            var products = new List<long>();
            products.AddRange(command.Entity.ProductIds);
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var exists = dbContext.Carts.FirstOrDefault(x => x.UserId == command.Entity.UserId);
            if (exists != null)
            {
                foreach (var item in products)
                {
                    exists.ProductIds.Remove(item);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

		#endregion
		#region Helpers
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
		private void Reattach(CartEntity entity, CartView view, AppDbContext dbContext)
		{
			CartMapper.From(view, entity);
		}
		#endregion
	}
}
