using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using System.Text.Json;
using Shared.Infrastructures;
using Service.Features.Order;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Shared.Infrastructures.Extensions;

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
            var isValid = ValidateToken(options.token);
            if (!IsAdminUser(isValid))
            {
                throw new CustomException("Not Permission");
            }
            await Invalidate();
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var orders = from s in dbContext.Orders select s;
            if (!String.IsNullOrEmpty(options.search))
            {
                orders = orders.Where(s =>
                         s.Region.Contains(options.search)
                         || s.FirstName.Contains(options.search)
                         || s.LastName.Contains(options.search)
                );
            }

            Sorting(ref orders, options);

            var count = await orders.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
            var items = await orders.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
            decimal totalPage = (decimal)count / (decimal)options.page_size;
            return new TableResponse<OrderView>() { Items = items.MapToViewList(), TotalItems = count, AllPage = (int)Math.Ceiling(totalPage), CurrentPage = options.page };

        }

        public async virtual Task<OrderResponse> Get(string token)
        {
            var isValid = ValidateToken(token);
            var isUser = IsUser(isValid);
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var order = await dbContext.Orders
                .FirstOrDefaultAsync(x => x.UserId == isUser.Id);

            var orderResponse = new OrderResponse();

            if (order != null)
            {
                orderResponse = order.MapToView2();
                var jsonx = System.Text.RegularExpressions.Regex.Unescape(order.Products);
                var lists = JsonSerializer.Deserialize<List<ProductResultView>>(jsonx);
                orderResponse.Product = lists;
            }
            else
            {
                throw new CustomException("OrderEntity Not Found");
            }

            return orderResponse;
        }



        public async virtual Task<OrderResponse> GetForAdmin(string token, long OrderId, CancellationToken cancellationToken = default)
        {
            var isValid = ValidateToken(token);
            var isUser = IsAdminUser(token);
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var order = await dbContext.Orders
                .FirstOrDefaultAsync(x => x.Id == OrderId);

            var orderResponse = new OrderResponse();

            if (order != null)
            {
                orderResponse = order.MapToView2();
                var jsonx = System.Text.RegularExpressions.Regex.Unescape(order.Products);
                var lists = JsonSerializer.Deserialize<List<ProductResultView>>(jsonx);
                orderResponse.Product = lists;
            }
            else
            {
                throw new CustomException("OrderEntity Not Found");
            }

            return orderResponse;
        }

        #endregion
        #region Mutations
        public async virtual Task Create(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            var valid = ValidateToken(command.Entity.Token);
            var isUser = IsUser(valid);
            command.Entity.UserId = isUser.Id;
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            //command.Entity.UserId = 
            var token = command.Entity.Token;
            var products = await cartService.GetAll(token, cancellationToken);
            var productResults = products.Items;
            string status = command.Entity.Status = OrderStatus.Pending.ToString().ToLower();
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var orderEntity = new OrderEntity
            {
                Products = JsonSerializer.Serialize(productResults),
                Status = status
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
            await cartService.RemoveAll(token, cancellationToken);
            await dbContext.SaveChangesAsync();
        }

        public async virtual Task Update(UpdateOrderCommand command, CancellationToken cancellationToken = default)
        {
            var isValid = ValidateToken(command.Entity.Token);
            var isUser = IsUser(isValid);
            if (!IsAdminUser(isValid))
            {
                throw new CustomException("User does not have permission to create a product.");
            }
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);

            var existingOrder = await dbContext.Orders
                .FirstOrDefaultAsync(x => x.UserId == isUser.Id);

            if (existingOrder == null)
            {
                throw new CustomException("OrderEntity Not Found");
            }

            existingOrder.City = command.Entity.City;
            existingOrder.Region = command.Entity.Region;
            existingOrder.Street = command.Entity.Street;
            existingOrder.HomeNumber = command.Entity.HomeNumber;
            existingOrder.CommentForCourier = command.Entity.CommentForCourier;
            existingOrder.DeliveryTime = command.Entity.DeliveryTime;
            existingOrder.PaymentType = command.Entity.PaymentType;
            existingOrder.FirstName = command.Entity.FirstName;
            existingOrder.LastName = command.Entity.LastName;
            existingOrder.ExtraPhoneNumber = command.Entity.ExtraPhoneNumber;
            existingOrder.Status = command.Entity.Status;

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        public async virtual Task Delete(DeleteOrderCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region Helpers
        private void Sorting(ref IQueryable<OrderEntity> unit, TableOptions options) => unit = options.sort_label switch
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
        #region Token Helper
        private bool IsAdminUser(string phoneNumber)
        {
            using var dbContext = dbHub.CreateDbContext();
            var user = dbContext.UsersEntities.FirstOrDefault(x => x.PhoneNumber == phoneNumber && x.Role == "Admin");
            return user != null;
        }
        private UserEntity IsUser(string phoneNumber)
        {
            using var dbContext = dbHub.CreateDbContext();
            var user = dbContext.UsersEntities.FirstOrDefault(x => x.PhoneNumber == phoneNumber && x.Role == "User");
            return user ?? throw new CustomException("Not Permission");
        }
        private string ValidateToken(string? token)
        {
            if (token == null)
            {
                throw new CustomException("Token is required");
            }
            var jwtEncodedString = token.Substring(7);

            var secondToken = new JwtSecurityToken(jwtEncodedString);
            var json = secondToken.Payload.Values.FirstOrDefault();
            if (json == null)
                throw new CustomException("Payload is null");
            else
            {
                return json?.ToString() ?? string.Empty;
            }
        }
        #endregion
    }
}
