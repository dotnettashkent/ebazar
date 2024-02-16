using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Shared.Infrastructures;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Shared.Infrastructures.Extensions;

namespace Service.Features
{
	public class ProductSubCategoryService : IProductSubCategoryService
	{
		#region Initialize
		private readonly DbHub<AppDbContext> _dbHub;

		public ProductSubCategoryService(DbHub<AppDbContext> dbHub)
		{
			_dbHub = dbHub;
		}
		#endregion
		#region Queries
		public async virtual Task<TableResponse<ProductSubCategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
            var isValid = ValidateToken(options.token);
            if (!IsAdminUser(isValid))
            {
                throw new CustomException("User does not have permission to create a product.");
            }
            await Invalidate();
			var dbContext = _dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var category = from s in dbContext.ProductSubCategories select s;

			if (!String.IsNullOrEmpty(options.search))
			{
				category = category.Where(s =>
						 s.Name.Contains(options.search)
				);
			}

			Sorting(ref category, options);

			var count = await category.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
			var items = await category.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
			return new TableResponse<ProductSubCategoryView>() { Items = items.MapToViewList(), TotalItems = count };
		}
		public async virtual Task<ProductSubCategoryView> Get(long Id, string token, CancellationToken cancellationToken = default)
		{
            var isValid = ValidateToken(token);
            if (!IsAdminUser(isValid))
            {
                throw new CustomException("User does not have permission to create a product.");
            }
            var dbContext = _dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var category = await dbContext.ProductSubCategories
				.FirstOrDefaultAsync(x => x.Id == Id);

			return category == null ? throw new CustomException("ProductSubCategoryEntity Not Found") : category.MapToView();
		}
		#endregion
		#region Mutations
		public async virtual Task Create(CreateProductSubCategoryCommand command, CancellationToken cancellationToken = default)
		{
            var isValid = ValidateToken(command.Token);
            if (!IsAdminUser(isValid))
            {
                throw new CustomException("User does not have permission to create a product.");
            }
            if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}

			await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);
			ProductSubCategoryEntity category = new ProductSubCategoryEntity();
			Reattach(category, command.Entity, dbContext);

			dbContext.Update(category);
			await dbContext.SaveChangesAsync();
		}

		public async virtual Task Delete(DeleteProductSubCategoryCommand command, CancellationToken cancellationToken = default)
		{
            var isValid = ValidateToken(command.Token);
            if (!IsAdminUser(isValid))
            {
                throw new CustomException("User does not have permission to create a product.");
            }
            if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);
			var brand = await dbContext.ProductSubCategories
				.FirstOrDefaultAsync(x => x.Id == command.Id);
			if (brand == null) throw new CustomException("ProductSubCategoryEntity Not Found");
			dbContext.Remove(brand);
			await dbContext.SaveChangesAsync();
		}


		public async virtual Task Update(UpdateProductSubCategoryCommand command, CancellationToken cancellationToken = default)
		{
            var isValid = ValidateToken(command.Token);
            if (!IsAdminUser(isValid))
            {
                throw new CustomException("User does not have permission to create a product.");
            }
            if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);
			var category = await dbContext.ProductSubCategories
				.FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

			if (category == null) throw new CustomException("ProductSubCategoryEntity Not Found");

			Reattach(category, command.Entity, dbContext);

			await dbContext.SaveChangesAsync();
		}
        #endregion
        #region Token
        private bool IsAdminUser(string phoneNumber)
        {
            using var dbContext = _dbHub.CreateDbContext();
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
        #endregion
        #region Helpers
        public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
		private void Reattach(ProductSubCategoryEntity entity, ProductSubCategoryView view, AppDbContext dbContext)
		{
			ProductSubCategoryMapper.From(view, entity);
		}
		private void Sorting(ref IQueryable<ProductSubCategoryEntity> entity, TableOptions options) => entity = options.sort_label switch
		{
			"Id" => entity.Ordering(options, o => o.Id),
			"Name" => entity.Ordering(options, o => o.Name),
			_ => entity.OrderBy(o => o.Id),

		};
		#endregion
	}
}
