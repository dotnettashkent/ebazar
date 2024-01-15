using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Service.Features.Order;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructures.Extensions;

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
                var productGetResult = await productService.GetById(item, cancellationToken);
                productList.Add(productGetResult);
            }
            var count = productList.Count();
            return new TableResponse<ProductResultView>() { Items = productList, TotalItems = count };
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

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var products = await dbContext.Products
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            var exists = dbContext.Orders.FirstOrDefault(x => x.UserId == command.Entity.UserId);
            if (exists != null)
            {
                exists.ProductIds.AddRange(command.Entity.ProductIds);
                var ids = exists.ProductIds.ToList();
                foreach (var item in ids)
                {
                    var prod = await productService.Get(item, cancellationToken);
                    prod.Count--;
                    prod.InfoCount++;
                }
            }
            else
            {
                var category = new OrderEntity();
                var productIds = command.Entity.ProductIds;
                foreach (var item in productIds)
                {
                    var product = await productService.Get(item, cancellationToken); 
                    product.Count--;
                    product.InfoCount++;
                }
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
