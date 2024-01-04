using Stl.Async;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Service.Features.Cart;
using Shared.Infrastructures;
using Stl.Fusion.EntityFramework;
using Shared.Infrastructures.Extensions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Stl.Fusion;
using Stl.Collections;

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
			var cart = new CartEntity();
			var ids = command.Entity.ProductIds;
			var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id.ToString() == command.Entity.ProductIds.FirstOrDefault());

			List<string> a = cart.ProductIds;
			a.

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
