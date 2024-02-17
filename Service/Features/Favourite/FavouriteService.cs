using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Stl.Fusion.EntityFramework;
using System.IdentityModel.Tokens.Jwt;
using Shared.Infrastructures.Extensions;

namespace Service.Features
{
    public class FavouriteService : IFavouriteService
    {
        #region Initialize
        private readonly DbHub<AppDbContext> dbHub;
        private readonly IProductService productService;
        public FavouriteService(DbHub<AppDbContext> dbHub, IProductService productService)
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
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var favourites = from s in dbContext.Favorites select s;
            favourites = favourites.Where(x => x.UserId == isUser.Id);

            var listProductId = new List<long>();
            foreach (var item in favourites)
            {
                listProductId.AddRange(item.Products);
            }
            var productList = new List<ProductResultView>();
            foreach (var item in listProductId)
            {
                var productGetResult = await productService.GetById(item, cancellationToken);
                productList.Add(productGetResult);
            }
            if (productList.Count == 0)
            {
                throw new CustomException("Favourite Not Found");
            }
            var count = productList.Count();
            return new TableResponse<ProductResultView>() { Items = productList, TotalItems = count };
        }

        #endregion
        #region Mutations
        public async virtual Task Create(CreateFavouriteCommand command, CancellationToken cancellationToken = default)
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
            var exists = dbContext.Favorites.FirstOrDefault(x => x.UserId == isUser.Id);
            if (exists != null)
            {
                exists.Products.AddRange(command.Entity.Products);
            }
            else
            {
                FavouriteEntity category = new FavouriteEntity();
                Reattach(category, command.Entity, dbContext);
                dbContext.Update(category);
            }

            await dbContext.SaveChangesAsync();
        }

        public async virtual Task Delete(DeleteFavouriteCommand command, CancellationToken cancellationToken = default)
        {
            var valid = ValidateToken(command.Entity.Token);
            var isUser = IsUser(valid);
            command.Entity.UserId = isUser.Id;
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            var products = new List<long>();
            products.AddRange(command.Entity.Products);
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var exists = dbContext.Favorites.FirstOrDefault(x => x.UserId == isUser.Id);
            if (exists != null)
            {
                foreach (var item in products)
                {
                    exists.Products.Remove(item);
                }
            }
            else if(exists == null)
            {
                throw new CustomException("Favourite Not Found");
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
        #endregion
        #region Helpers
        public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

        private void Reattach(FavouriteEntity favourite, FavouriteView favouriteView, AppDbContext dbContext)
        {
            FavouriteMapper.From(favouriteView, favourite);
        }
        #endregion
        #region Token Helper
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
