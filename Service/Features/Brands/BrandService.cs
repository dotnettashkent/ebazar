using Stl.Async;
using Stl.Fusion;
using Service.Data;
using Shared.Features;
using System.Reactive;
using Shared.Infrastructures;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructures.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Service.Features
{
	public class BrandService : IBrandService
	{
		#region Initialize
		private readonly DbHub<AppDbContext> dbHub;
		private readonly IFileService fileService;

        public BrandService(DbHub<AppDbContext> dbHub, IFileService fileService)
        {
            this.dbHub = dbHub;
            this.fileService = fileService;
        }
        #endregion
        #region Queries

        public async virtual Task<TableResponse<BrandView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
		{
			await Invalidate();
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var brand = from s in dbContext.Brands select s;

			if (!String.IsNullOrEmpty(options.Search))
			{
				brand = brand.Where(s =>
						 s.Name.Contains(options.Search)
						|| s.IsPopular.Contains(options.Search)
				);
			}

			Sorting(ref brand, options);

			var count = await brand.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
			var items = await brand.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
			return new TableResponse<BrandView>() { Items = items.MapToViewList(), TotalItems = count };
		}
		public async virtual Task<BrandView> Get(long Id, CancellationToken cancellationToken = default)
		{
			var dbContext = dbHub.CreateDbContext();
			await using var _ = dbContext.ConfigureAwait(false);
			var category = await dbContext.Brands
			.FirstOrDefaultAsync(x => x.Id == Id);

			return category == null ? throw new ValidationException("BrandEntity Not Found") : category.MapToView();
		}
		#endregion

		#region Mutations
		public async virtual Task Create(CreateBrandCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			if(command.Entity.Photo != null)
			{
				var fileResult = await fileService.SaveImage(command.Entity.Photo);
				if (fileResult.Item1 == 1)
				{
					command.Entity.ImageOne = fileResult.Item2;
				}
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			BrandEntity category = new BrandEntity();
			Reattach(category, command.Entity, dbContext);

			dbContext.Update(category);
			await dbContext.SaveChangesAsync();
		}

		public async virtual Task Delete(DeleteBrandCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var brand = await dbContext.Brands
			.FirstOrDefaultAsync(x => x.Id == command.Id);
			if (brand == null) throw new ValidationException("BrandEntity Not Found");
			await fileService.DeleteImage(brand.ImageOne);
			dbContext.Remove(brand);
			await dbContext.SaveChangesAsync();
		}



		public async virtual Task Update(UpdateBrandCommand command, CancellationToken cancellationToken = default)
		{
			if (Computed.IsInvalidating())
			{
				_ = await Invalidate();
				return;
			}
			await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
			var brand = await dbContext.Brands
			.FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

			if (brand == null) throw new ValidationException("BrandEntity Not Found");

			Reattach(brand, command.Entity, dbContext);

			await dbContext.SaveChangesAsync();
		}
		#endregion

		#region Helpers
		public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
		private void Reattach(BrandEntity brand, BrandView brandView, AppDbContext dbContext)
		{
			BrandMapper.From(brandView, brand);
		}
		private void Sorting(ref IQueryable<BrandEntity> brand, TableOptions options) => brand = options.SortLabel switch
		{
			"Id" => brand.Ordering(options, o => o.Id),
			"Name" => brand.Ordering(options, o => o.Name),
			"Link" => brand.Ordering(options, o => o.Link),
			_ => brand.OrderBy(o => o.Id),

		};
		#endregion
	}
}
