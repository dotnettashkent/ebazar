using Microsoft.EntityFrameworkCore;
using Service.Data;
using Service.Features.Cart;
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
	public class CartService : ICartService
	{
		#region Initialize
		private readonly DbHub<AppDbContext> dbHub;

		public CartService(DbHub<AppDbContext> dbHub)
		{
			this.dbHub = dbHub;
		}
		#endregion

		#region Queries
		public async virtual Task<TableResponse<CartView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var cart = from s in dbContext.Carts select s;

			//Sorting(ref cart, options);

			var count = await cart.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
			var items = await cart.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
			return new TableResponse<CartView>() { Items = items.MapToViewList(), TotalItems = count };
		}
		public async virtual Task<CartView> Get(long Id, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var cart = await dbContext.Carts
				.Include(s => s.Products)
				.Include(s => s.User)
				.FirstOrDefaultAsync(x => x.Id == Id);

			return cart == null ? throw new ValidationException("CartEntity Not Found") : cart.MapToView();
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
			var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == command.productId);
			var cart = new CartEntity();
			cart.ProductIds.Add(product.Id.ToString());
			dbContext.Carts.Add(cart);
			await dbContext.SaveChangesAsync(cancellationToken);
		}


		public async virtual Task Delete(DeleteCartCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
		}


		public async virtual Task Update(UpdateCartCommand command, CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();
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
