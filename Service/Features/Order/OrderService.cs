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
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
            if (!String.IsNullOrEmpty(options.Search))
            {
                orders = orders.Where(s =>
                         s.Region.Contains(options.Search)
                         || s.FirstName.Contains(options.Search)
                         || s.LastName.Contains(options.Search)
                );
            }

            Sorting(ref orders, options);

            var count = await orders.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
            var items = await orders.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
            return new TableResponse<OrderView>() { Items = items.MapToViewList(), TotalItems = count };

        }

        public async virtual Task<OrderResponse> Get(long Id, CancellationToken cancellationToken = default)
        {
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);

            // Query the order without including related products
            var order = await dbContext.Orders
                .FirstOrDefaultAsync(x => x.Id == Id);

            var orderResponse = new OrderResponse();

            if (order != null)
            {
                // Deserialize the JSON string to get the related products
                orderResponse = order.MapToView2();
                var jsonx = System.Text.RegularExpressions.Regex.Unescape(order.Products);
                var lists = JsonSerializer.Deserialize<List<ProductResultView>>(jsonx); 
                orderResponse.Product = lists;
            }
            else
            {
                throw new ValidationException("OrderEntity Not Found");
            }

            return orderResponse;
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
        private void Sorting(ref IQueryable<OrderEntity> unit, TableOptions options) => unit = options.SortLabel switch
        {
            "City" => unit.Ordering(options, o => o.City),
            "Region" => unit.Ordering(options, o => o.Region),
            "Status" => unit.Ordering(options, o => o.Status),
            "PaymentType" => unit.Ordering(options, o => o.PaymentType),
            "Street" => unit.Ordering(options, o => o.Street),
            "FirstName" => unit.Ordering(options, o => o.FirstName),
            "LastName" => unit.Ordering(options, o => o.LastName),
            _ => unit.OrderBy(o => o.Id),
        };
        public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
        private void Reattach(OrderEntity entity, OrderView view, AppDbContext dbContext)
        {
            OrderMapper.From(view, entity);
        }

        #endregion

    }
}
