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
		public virtual async Task<ProductCategoryEntity?> Get(long Id, string locale, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var section = from s in dbContext.ProductCategories select s;
			section = section
				.Include(x => x.Photo)
				.Include(x => x.PhotoMobile);
			return await section.FirstOrDefaultAsync(x => x.Id == Id && x.Locale == locale);
		}

		[ComputeMethod]
		public virtual async Task<TableResponse<ProductCategoryView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();

			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var categories = from s in dbContext.ProductCategories select s;

			if (!String.IsNullOrEmpty(options.Search))
			{
				categories = categories.Where(s =>
						 s.MainLinkEn.Contains(options.Search)
						|| s.MainLinkRu.Contains(options.Search)
						|| s.MainNameUz.Contains(options.Search)
				);
			}

			var count = await categories.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
			var items = await categories.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
			return new TableResponse<ProductCategoryView>() { Items = items.MapToViewList(), TotalItems = count };
		}

		public async virtual Task<ProductCategoryView> GetById(long id, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var category = await dbContext.ProductCategories
				.Include(x => x.Photo)
				.Include(x => x.PhotoMobile)
				.Where(x => x.Id == id).FirstOrDefaultAsync();

			return category == null ? throw new ValidationException("ProductCategoryEntity Not Found...") : category.MapToView();
		}
		#endregion
		#region Mutations
		public async virtual Task Create(CreateProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				foreach (var item in command.Entity)
				{
					_ = await Get(item.Id, item.Locale, cancellationToken);
				}
				_ = await Invalidate();
			}
			if (command.Entity.Count != 3)
				throw new Exception("Section must be 3 language!");

			await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);
			var maxId = await dbContext.Sections.Select(x => x.Id).MaxAsync();
			maxId++;
			try
			{
				foreach (var item in command.Entity)
				{
					SectionEntity section = new SectionEntity();
					section = Reattach(section, item, dbContext);
					section.Id = maxId;
					dbContext.Sections.Add(section);
				}
				await dbContext.SaveChangesAsync();
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
			return maxId;
		}

		public async virtual Task Delete(DeleteProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var category = await dbContext.ProductCategories
			.Include(x => x.Photo)
			.Include(x => x.PhotoMobile)
			.FirstOrDefaultAsync(x => x.Id == command.Id);
			if (category == null) throw new ValidationException("ProductCategoryEntity Not Found");
			dbContext.Remove(category);
			await dbContext.SaveChangesAsync();
		}

		public async virtual Task Update(UpdateProductCategoryCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var category = await dbContext.ProductCategories
			.Include(x => x.Photo)
			.Include(x => x.PhotoMobile)
			.FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

			if (category == null) throw new ValidationException("ProductCategoryEntity Not Found");

			Reattach(category, command.Entity, dbContext);

			await dbContext.SaveChangesAsync();
		}
		#endregion
		#region Helpers

		[ComputeMethod]
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;

		private void Reattach(ProductCategoryEntity category, ProductCategoryView categoryView, AppDbContext dbContext)
		{
			ProductCategoryMapper.From(categoryView, category);
		}
		#endregion
	}
}
