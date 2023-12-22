using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Shared.Infrastructures;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Service.Features.ProductCategory;
using Shared.Infrastructures.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Service.Features
{
	public class ProductCategoryService : IProductCategoryService
	{
		#region Initialize
		private readonly DbHub<AppDbContext> dbHub;
		public ProductCategoryService(DbHub<AppDbContext> dbHub)
		{
			this.dbHub = dbHub;
		}
		#endregion

		#region Queries
		

		[ComputeMethod]
		public virtual async Task<TableResponse<ProductCategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var category = from s in dbContext.ProductCategories select s;

			if (!String.IsNullOrEmpty(options.Search))
			{
				category = category.Where(s =>
					s.Id.ToString().Contains(options.Search)
				);
			}

			category = category.Include(x => x.Photo);
			category = category.Include(x => x.PhotoMobile);
			category = category.Where(x => x.Locale.Equals(options.Lang));

			Sorting(ref category, options);

			category = category.Paginate(options);

			List<ProductCategoryEntity> tags = new List<ProductCategoryEntity>();
			foreach (var item in category)
			{
				var tagsById = await GetFor(item.Id, item.Locale, cancellationToken);
				if (tagsById is not null)
					tags.Add(tagsById);
			}
			var count = tags.Count();
			return new TableResponse<ProductCategoryView>() { Items = tags.MapToViewList(), TotalItems = count };
		}

		[ComputeMethod]
		public async virtual Task<List<ProductCategoryView>> Get(long Id, CancellationToken cancellationToken = default)
		{
			await Invalidate();

			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);

			var tags = await dbContext.ProductCategories
				.Include(x => x.Photo)
				.Include(x => x.PhotoMobile)
				.Where(x => x.Id == Id)
				.ToListAsync(cancellationToken);

			return tags.Select(tag => tag.MapToView()).ToList();
		}
		#endregion
		#region Mutations
		public async virtual Task Create(CreateProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
			}

			if (command.Entity.Count != 3)
				throw new Exception("ProductCategory must be in 3 languages!");

			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);

			try
			{
				var maxId = await dbContext.ProductCategories.Select(x => x.Id).MaxAsync();
				maxId++;

				foreach (var item in command.Entity)
				{
					ProductCategoryEntity category = new ProductCategoryEntity();
					category = Reattach(category, item, dbContext);
					category.Id = maxId;
					dbContext.ProductCategories.Add(category);
				}

				await dbContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				throw;
			}
		}

		public async virtual Task Delete(DeleteProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var tag = await dbContext.ProductCategories
			.Where(x => x.Id == command.Id).ToListAsync();

			if (tag == null) 
				throw new ValidationException("ProductCategoryEntity Not Found");
			dbContext.Remove(tag);
			await dbContext.SaveChangesAsync();
		}

		public async virtual Task Update(UpdateProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				foreach (var entity in command.Entity)
				{
					_ = await GetFor(entity.Id, entity.Locale, cancellationToken);
				}
				return;
			}

			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);

			foreach (var entityToUpdate in command.Entity)
			{
				var tag = await dbContext.ProductCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entityToUpdate.Id && x.Locale == entityToUpdate.Locale);

				if (tag == null)
				{
					throw new ValidationException($"ProductCategoryEntity with Id {entityToUpdate.Id} not found");
				}

				Reattach(tag, entityToUpdate, dbContext);

				dbContext.Update(tag);
			}

			await dbContext.SaveChangesAsync();
		}

		[ComputeMethod]
		public async virtual Task<ProductCategoryEntity?> GetFor(long id, string locale, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);

			var tag = await dbContext.ProductCategories
				.Include(x => x.Photo)
				.Include(x => x.PhotoMobile)
				.FirstOrDefaultAsync(x => x.Id == id && x.Locale == locale);

			return tag is null ? null : tag;
		}
		#endregion
		#region Helpers

		[ComputeMethod]
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

		private ProductCategoryEntity Reattach(ProductCategoryEntity entity, ProductCategoryView view, AppDbContext dbContext)
		{
			ProductCategoryMapper.From(view, entity);
			return entity;

		}

		private void Sorting(ref IQueryable<ProductCategoryEntity> tag, TableOptions options) => tag = options.SortLabel switch
		{
			"Id" => tag.Ordering(options, o => o.Id),
			"Status" => tag.Ordering(options, o => o.IsPopular),
			_ => tag.OrderBy(o => o.Id),

		};
		#endregion
	}
}
