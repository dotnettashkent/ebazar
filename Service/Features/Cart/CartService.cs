using Stl.Async;
using Stl.Fusion;
using Service.Data;
using System.Reactive;
using Shared.Features;
using System.Text.Json;
using Service.Features.Cart;
using Stl.Fusion.EntityFramework;
using System.IdentityModel.Tokens.Jwt;
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
        public async virtual Task<TableResponse<ProductResultView>> GetAll(string token, CancellationToken cancellationToken = default)
        {
            var valid = ValidateToken(token);
            var isUser = IsUser(valid);
            await Invalidate();
            await using var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext;
            var cart = dbContext.Carts.FirstOrDefault(x => x.UserId == isUser.Id);

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
            return new TableResponse<ProductResultView>();
            //throw new CustomException("CartEntity Not Found");
        }



        #endregion
        #region Mutations
        public async virtual Task Create(CreateCartCommand command, CancellationToken cancellationToken = default)
        {
            var valid = ValidateToken(command.Entity.Token);
            var isUser = IsUser(valid);
            command.Entity.UserId = isUser.Id;
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var exists = dbContext.Carts.FirstOrDefault(x => x.UserId == isUser.Id);

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

        public async virtual Task UpdateProductQuantityAsync(UpdateCartCommand command, CancellationToken cancellationToken = default)
        {
            var isValid = ValidateToken(command.Entity.Token!);

            if (IsAdminUser(isValid))
            {
                throw new CustomException("Admin does not have permission to update a product quantity.");
            }
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);

            var isUser = IsUser(isValid);
            var cart = dbContext.Carts.FirstOrDefault(x => x.UserId == isUser.Id);

            if (cart != null && cart.Product != null)
            {
                var cartProducts = JsonSerializer.Deserialize<List<ProductList>>(cart.Product);
                UpdateProductQuantitiesInCart(cartProducts, command.Entity.Products);
                cart.Product = JsonSerializer.Serialize(cartProducts);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                throw new CustomException("Cart not found for the user.");
            }
        }

        private static void UpdateProductQuantitiesInCart(List<ProductList> cartProducts, IEnumerable<ProductList> updateProducts)
        {
            foreach (var updateProduct in updateProducts)
            {
                var existingProduct = cartProducts.FirstOrDefault(p => p.ProductId == updateProduct.ProductId);

                if (existingProduct != null)
                {
                    existingProduct.Quantity -= updateProduct.Quantity;

                    if (existingProduct.Quantity <= 0)
                    {
                        cartProducts.Remove(existingProduct);
                    }
                }
            }
        }



        public async virtual Task Delete(DeleteCartCommand command, CancellationToken cancellationToken = default)
        {
            var valid = ValidateToken(command.Entity.Token);
            var isUser = IsUser(valid);
            command.Entity.UserId = isUser.Id;
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var cart = dbContext.Carts.FirstOrDefault(x => x.UserId == isUser.Id);
            if (cart == null)
            {
                throw new CustomException("CartEntity Not Found");
            }
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
                    existingProduct.Quantity = Math.Max(0, existingProduct.Quantity - productToRemove.Quantity);

                    if (existingProduct.Quantity == 0)
                    {
                        cartProducts.Remove(existingProduct);
                    }
                }
            }
        }

        public async virtual Task RemoveAll(string token, CancellationToken cancellationToken = default)
        {
            var valid = ValidateToken(token);
            var isUser = IsUser(valid);
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);

            // Find all carts with the specified userId
            var cartsToDelete = dbContext.Carts.Where(x => x.UserId == isUser.Id).ToList();

            foreach (var cart in cartsToDelete)
            {
                dbContext.Remove(cart);
            }

            await dbContext.SaveChangesAsync();
        }




        #endregion
        #region Helpers

        private bool IsAdminUser(string phoneNumber)
        {
            using var dbContext = dbHub.CreateDbContext();
            var user = dbContext.UsersEntities.FirstOrDefault(x => x.PhoneNumber == phoneNumber && x.Role == "Admin");
            return user != null;
        }
        public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
        private void Reattach(CartEntity entity, CartView view, AppDbContext dbContext)
        {
            CartMapper.From(view, entity);
        }
        #endregion
        #region Token
        private UserEntity IsUser(string phoneNumber)
        {
            using var dbContext = dbHub.CreateDbContext();
            var user = dbContext.UsersEntities.FirstOrDefault(x => x.PhoneNumber == phoneNumber && x.Role == "User");
            return user ?? throw new CustomException("Not Permission");
        }
        private string ValidateToken(string token)
        {
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
