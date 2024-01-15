using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Service.Features.Order;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructures.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Service.Features
{
    public class OrderService : IOrderServices
    {
        #region Initialize
        private readonly DbHub<AppDbContext> dbHub;
        private readonly IProductService productService;
        public OrderService(DbHub<AppDbContext> dbHub, IProductService productService)
        {
            this.dbHub = dbHub;
            this.productService = productService;
        }
        #endregion
        #region Queries
        public async virtual Task<TableResponse<ProductResultView>> GetAll(long  UserId, CancellationToken cancellationToken = default)
        {
            await Invalidate();
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var favourites = from s in dbContext.Orders select s;
            favourites = favourites.Where(x => x.UserId == UserId);

            var listProductId = new List<long>();
            foreach (var item in favourites)
            {
                listProductId.AddRange(item.ProductIds);
            }
            var productList = new List<ProductResultView>();
            foreach (var item in listProductId)
            {
                var productGetResult = productService.GetById(item, cancellationToken).Result;
                productList.Add(productGetResult);
            }
            var count = productList.Count();
            return new TableResponse<ProductResultView>() { Items = productList, TotalItems = count };
        }

        /*public async virtual Task<ProductResultView> Get(long Id, CancellationToken cancellationToken = default)
        {
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var order = await dbContext.Orders
            .FirstOrDefaultAsync(x => x.Id == Id);

            return order == null ? throw new ValidationException("OrderEntity Not Found") : order.MapToView();
        }*/
        #endregion

        #region Mutations
        public async virtual Task Create(CreateOrderCommand command, CancellationToken cancellationToken = default)
        {
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var exists = dbContext.Orders.FirstOrDefault(x => x.UserId == command.Entity.UserId);
            if (exists != null)
            {
                exists.ProductIds.AddRange(command.Entity.ProductIds);
            }
            else
            {
                var category = new OrderEntity();
                Reattach(category, command.Entity, dbContext);
                dbContext.Update(category);
            }

            await dbContext.SaveChangesAsync();
        }

        public async virtual Task Delete(DeleteOrderCommand command, CancellationToken cancellationToken = default)
        {

            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            var products = new List<long>();
            products.AddRange(command.Entity.ProductIds);
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var exists = dbContext.Orders.FirstOrDefault(x => x.UserId == command.Entity.UserId);
            if (exists != null)
            {
                foreach (var item in products)
                {
                    exists.ProductIds.Remove(item);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async virtual Task Update(UpdateOrderCommand command, CancellationToken cancellationToken = default)
        {
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var order = await dbContext.Orders
            .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

            if (order == null) throw new ValidationException("OrderEntity Not Found");

            Reattach(order, command.Entity, dbContext);

            await dbContext.SaveChangesAsync();
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
