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
using System.Text.Json;

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

            var userId = command.Entity.UserId;
            var products = await cartService.GetAll(userId, cancellationToken);
            var productResults = products.Items;

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var orderEntity = new OrderEntity
            {
                Products = JsonSerializer.Serialize(productResults),
            };
            Reattach(orderEntity, command.Entity, dbContext);
            dbContext.Orders.Add(orderEntity);
            foreach (var item2 in productResults)
            {
                var product = dbContext.Products.SingleOrDefault(p => p.Id == item2.Id);
                if (product != null)
                {
                    product.Count -= item2.Quantity;
                    product.InfoCount += item2.Quantity;
                }
            }
            await cartService.RemoveAll(userId, cancellationToken);
            await dbContext.SaveChangesAsync();
        }

        public async virtual Task Update(UpdateOrderCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
