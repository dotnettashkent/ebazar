using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Data;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.Fusion;
using Stl.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Reactive;

namespace Service.Features
{
    public class ProductService : IProductService
    {
        #region Initialize
        private readonly DbHub<AppDbContext> dbHub;
        private readonly IFileService fileService;
        private readonly IConfiguration configuration;

        public ProductService(DbHub<AppDbContext> dbHub, IFileService fileService, IConfiguration configuration)
        {
            this.dbHub = dbHub;
            this.fileService = fileService;
            this.configuration = configuration;
        }
        #endregion
        #region Queries
        public async virtual Task<TableResponse<ProductResultView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
        {
            await Invalidate();
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var product = from s in dbContext.Products select s;
            if (!String.IsNullOrEmpty(options.search))
            {
                product = product.Where(s =>
                         s.NameUz != null && s.NameUz.Contains(options.search)
                         || s.NameRu.Contains(options.search)
                );
            }

            Sorting(ref product, options);


            var count = await product.AsNoTracking().CountAsync();
            var items = await product.AsNoTracking().Paginate(options).ToListAsync();
            decimal totalPage = (decimal)count / (decimal)options.page_size;
            return new TableResponse<ProductResultView>() { Items = items.MapToViewListResult(), TotalItems = count, AllPage = (int)Math.Ceiling(totalPage), CurrentPage = options.page };
        }
        public async virtual Task<ProductResultView> GetById(long Id, CancellationToken cancellationToken = default)
        {
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var product = await dbContext.Products
                .FirstOrDefaultAsync(x => x.Id == Id);

            return product == null ? throw new CustomException("ProductEntity Not Found") : product.MapToView();
        }

        public async virtual Task<ProductView> Get(long Id, CancellationToken cancellationToken = default)
        {
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var product = await dbContext.Products
                .FirstOrDefaultAsync(x => x.Id == Id);

            return product == null ? throw new ValidationException("ProductEntity Not Found") : product.MapToView2();
        }
        #endregion

        #region Mutations
        public async virtual Task Create(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            var phoneNumber = ValidateToken(command.Token);
            if (!IsAdminUser(phoneNumber))
            {
                throw new CustomException("User does not have permission to create a product.");
            }
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            #region Check image

            if (command.Entity.ImageOne != null)
            {
                var fileResult = await fileService.SaveImage(command.Entity.ImageOne);
                if (fileResult.Item1 == 1)
                {
                    command.Entity.PhotoOne = fileResult.Item2;
                }
            }
            if (command.Entity.ImageTwo != null)
            {
                var fileResult = await fileService.SaveImage(command.Entity.ImageTwo);
                if (fileResult.Item1 == 1)
                {
                    command.Entity.PhotoTwo = fileResult.Item2;
                }
            }
            if (command.Entity.ImageThree != null)
            {
                var fileResult = await fileService.SaveImage(command.Entity.ImageThree);
                if (fileResult.Item1 == 1)
                {
                    command.Entity.PhotoThree = fileResult.Item2;
                }
            }

            if (command.Entity.ImageFour != null)
            {
                var fileResult = await fileService.SaveImage(command.Entity.ImageFour);
                if (fileResult.Item1 == 1)
                {
                    command.Entity.PhotoFour = fileResult.Item2;
                }
            }

            if (command.Entity.ImageFive != null)
            {
                var fileResult = await fileService.SaveImage(command.Entity.ImageFive);
                if (fileResult.Item1 == 1)
                {
                    command.Entity.PhotoFive = fileResult.Item2;
                }
            }
            if (command.Entity.ImageSix != null)
            {
                var fileResult = await fileService.SaveImage(command.Entity.ImageSix);
                if (fileResult.Item1 == 1)
                {
                    command.Entity.PhotoSix = fileResult.Item2;
                }
            }

            #endregion 
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            ProductEntity entity = new ProductEntity();
            Reattach(entity, command.Entity, dbContext);

            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async virtual Task Delete(DeleteProductCommand command, CancellationToken cancellationToken = default)
        {
            var phoneNumber = ValidateToken(command.Token);
            if (!IsAdminUser(phoneNumber))
            {
                throw new CustomException("User does not have permission to delete a product.");
            }
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var entity = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == command.Id);

            if (entity == null)
                throw new CustomException("ProductEntity Not Found");
            dbContext.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
        public async virtual Task Update(UpdateProductCommand command, CancellationToken cancellationToken = default)
        {
            var phoneNumber = ValidateToken(command.Token);
            if (!IsAdminUser(phoneNumber))
            {
                throw new CustomException("User does not have permission to update a product.");
            }
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var entity = await dbContext.Products
                .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

            if (entity == null)
                throw new CustomException("ProductEntity Not Found");

            Reattach(entity, command.Entity, dbContext);

            await dbContext.SaveChangesAsync();
        }

        #endregion

        #region Helpers
        //[ComputeMethod]
        public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

        private ProductEntity Reattach(ProductEntity entity, ProductResultView view, AppDbContext dbContext)
        {
            ProductMapper.From2(view, entity);
            return entity;

        }
        private bool IsAdminUser(string phoneNumber)
        {
            using var dbContext = dbHub.CreateDbContext();
            var user = dbContext.UsersEntities.FirstOrDefault(x => x.PhoneNumber == phoneNumber && x.Role == "Admin");
            return user != null;
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
        private void Sorting(ref IQueryable<ProductEntity> offering, TableOptions options) => offering = options.sort_label switch
        {
            "name_uz" => offering.Ordering(options, o => o.NameUz),
            "name_ru" => offering.Ordering(options, o => o.NameRu),
            "brand_name" => offering.Ordering(options, o => o.BrandName),
            "count" => offering.Ordering(options, o => o.Count),
            "info_count" => offering.Ordering(options, o => o.InfoCount),
            "price" => offering.Ordering(options, o => o.Price),
            "price_type" => offering.Ordering(options, o => o.PriceType),
            "is_delivery_free" => offering.Ordering(options, o => o.IsDeliveryFree),
            "tag" => offering.Ordering(options, o => o.Tag),
            "unit" => offering.Ordering(options, o => o.Unit),
            "is_active" => offering.Ordering(options, o => o.IsActive),
            "category" => offering.Ordering(options, o => o.Category),
            "created_at" => offering.Ordering(options, o => o.CreatedAt),
            _ => offering.OrderByDescending(o => o.Id)
        };
        #endregion

    }
}
