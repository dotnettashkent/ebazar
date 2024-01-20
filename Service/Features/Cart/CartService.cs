using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Service.Features.Cart;
using Stl.Fusion.EntityFramework;
using Shared.Infrastructures.Extensions;
using System.Text.Json;

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
        public async virtual Task<TableResponse<ProductResultView>> GetAll(long userId, CancellationToken cancellationToken = default)
        {
            await Invalidate();

            await using var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext;

            var cart = dbContext.Carts.FirstOrDefault(x => x.UserId == userId);

            if (cart != null && cart.Product != null)
            {
                var productRes = new List<ProductResultView>();
                var cartView = JsonSerializer.Deserialize<List<ProductList>>(cart.Product);
                
                foreach (var item in cartView)
                {
                    var productResult = await productService.GetById(item.ProductId);
                    productResult.Quantity = item.Quantity;
                    productRes.Add(productResult);
                }

                var count = productRes.Count();
                return new TableResponse<ProductResultView>() { Items = productRes, TotalItems = count };
            }

            // If the cart is empty or doesn't exist
            return new TableResponse<ProductResultView>() { Items = new List<ProductResultView>(), TotalItems = 0 };
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

            if (exists != null && exists.Product != null)
            {
                var cartProducts = JsonSerializer.Deserialize<List<ProductList>>(exists.Product);
                UpdateProductsInCart(cartProducts, command.Entity.Products);
                exists.Product = JsonSerializer.Serialize(cartProducts);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                CartEntity cart = new CartEntity();
                var cartProducts = new List<ProductList>();
                UpdateProductsInCart(cartProducts, command.Entity.Products);
                cart.Product = JsonSerializer.Serialize(cartProducts);
                Reattach(cart, command.Entity, dbContext);
                dbContext.Update(cart);
                await dbContext.SaveChangesAsync();
            }
        }

        private static void UpdateProductsInCart(List<ProductList> cartProducts, IEnumerable<ProductList> newProducts)
        {
            foreach (var newProduct in newProducts)
            {
                var existingProduct = cartProducts.FirstOrDefault(p => p.ProductId == newProduct.ProductId);

                if (existingProduct != null)
                {
                    existingProduct.Quantity += newProduct.Quantity;
                }
                else
                {
                    cartProducts.Add(new ProductList
                    {
                        ProductId = newProduct.ProductId,
                        Quantity = newProduct.Quantity
                    });
                }
            }
        }




        public async virtual Task Delete(DeleteCartCommand command, CancellationToken cancellationToken = default)
        {
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var cart = dbContext.Carts.FirstOrDefault(x => x.UserId == command.Entity.UserId);

            if (cart != null && cart.Product != null)
            {
                var cartProducts = JsonSerializer.Deserialize<List<ProductList>>(cart.Product);
                RemoveProductsFromCart(cartProducts, command.Entity.Products);
                cart.Product = JsonSerializer.Serialize(cartProducts);
                await dbContext.SaveChangesAsync();
            }
        }

        private static void RemoveProductsFromCart(List<ProductList> cartProducts, IEnumerable<ProductList> productsToRemove)
        {
            foreach (var productToRemove in productsToRemove)
            {
                var existingProduct = cartProducts.FirstOrDefault(p => p.ProductId == productToRemove.ProductId);

                if (existingProduct != null)
                {
                    // Remove the specified quantity or the whole product from the cart
                    existingProduct.Quantity = Math.Max(0, existingProduct.Quantity - productToRemove.Quantity);

                    // If the quantity becomes zero, remove the product from the cart
                    if (existingProduct.Quantity == 0)
                    {
                        cartProducts.Remove(existingProduct);
                    }
                }
            }
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
