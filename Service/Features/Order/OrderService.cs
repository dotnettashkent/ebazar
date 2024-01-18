using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Service.Features.Order;
using Stl.Fusion.EntityFramework;
using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using System.Net;

namespace Service.Features
{
    public class OrderService : IOrderServices
    {
        #region Initialize
        private readonly DbHub<AppDbContext> dbHub;
        private readonly IProductService productService;
        private readonly ICartService cartService;
        private readonly IUserService userService;
        public OrderService(DbHub<AppDbContext> dbHub, IProductService productService, ICartService cartService, IUserService userService)
        {
            this.dbHub = dbHub;
            this.productService = productService;
            this.cartService = cartService;
            this.userService = userService;
        }
        #endregion
        #region Queries
        public async virtual Task<TableResponse<OrderView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
        {
            await Invalidate();
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var orders = from s in dbContext.Orders select s;
            var ordersResponse = new List<OrderView>();
            /*foreach (var order in orders)
            {
                var userId = order.UserId;
                var cart =  await cartService.GetAll(userId,cancellationToken);
                foreach (var item in cart.Items)
                {
                    order.ProductIds.Add(item.Id);
                }
            }*/

            return null;

        }

        public Task<OrderView> Get(long Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Mutations
        public async virtual Task Create(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            
            var userProducts = await cartService.GetAll(command.Entity.UserId,cancellationToken);
            foreach (var item in userProducts.Items)
            {
                command.Entity.ProductIds.Add(item.Id);
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var order = new OrderEntity();
            Reattach(order,command.Entity, dbContext);
            var sameProduct = dbContext.Products.AsQueryable();
            foreach (var item in sameProduct)
            {
                foreach (var item2 in command.Entity.ProductIds)
                {
                    if (item.Id == item2)
                    {
                        item.InfoCount++;
                        item.Count--;
                    }
                }
                await dbContext.SaveChangesAsync();
            }
            dbContext.Update(order);
            await dbContext.SaveChangesAsync();
        }

        public async virtual Task Delete(DeleteOrderCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Helpers

        public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
        private void Reattach(OrderEntity entity, OrderView view, AppDbContext dbContext)
        {
            OrderMapper.From(view, entity);
        }
        #endregion

    }
}
