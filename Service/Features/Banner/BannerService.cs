using Stl.Async;
using Stl.Fusion;
using Service.Data;
using System.Reactive;
using Shared.Features;
using Service.Features;
using Shared.Features.Banner;
using Shared.Infrastructures;
using Service.Features.Banner;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructures.Extensions;
using System.ComponentModel.DataAnnotations;
namespace UtcNew.Services;

public class BannerService : IBannerService
{
	#region Initialize
	private readonly DbHub<AppDbContext> _dbHub;

	public BannerService(DbHub<AppDbContext> dbHub)
	{
		_dbHub = dbHub;
	}
	#endregion
	#region Queries
	//[ComputeMethod]
	public virtual async Task<TableResponse<BannerView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
	{
		await Invalidate();
		var dbContext = _dbHub.CreateDbContext();
		await using var _ = dbContext.ConfigureAwait(false);
		var Banner = from s in dbContext.Banners select s;

		if (!String.IsNullOrEmpty(options.Search))
		{
			Banner = Banner.Where(s =>
					 s.Title.Contains(options.Search)
					|| s.Description.Contains(options.Search)
			);
		}

		Sorting(ref Banner, options);

		Banner = Banner.Where(x => x.Locale.Equals(options.Lang));
		Banner = Banner.Include(x => x.Photo);
		var count = await Banner.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
		var items = await Banner.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
		return new TableResponse<BannerView>() { Items = items.MapToViewList(), TotalItems = count };
	}

	//[ComputeMethod]
	public async virtual Task<List<BannerView>> Get(long Id, CancellationToken cancellationToken = default)
	{
		var dbContext = _dbHub.CreateDbContext();
		await using var _ = dbContext.ConfigureAwait(false);
		var Banner = await dbContext.Banners
		.Include(x => x.Photo)
		.Where(x => x.Id == Id).ToListAsync();

		return Banner == null ? throw new ValidationException("BannerEntity Not Found") : Banner.MapToViewList();
	}
	#endregion
	#region Mutations
	public virtual async Task Create(CreateBannerCommand command, CancellationToken cancellationToken = default)
	{
		if (Computed.IsInvalidating())
		{
			_ = await Invalidate();
			return;
		}

		await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);
		long maxId = 1;
		var entity = await dbContext.Banners.OrderByDescending(entity => entity.Id).FirstOrDefaultAsync(cancellationToken);
		if (entity != null) maxId = entity.Id + 1;

		foreach (var item in command.Entity)
		{
			var Banner = new BannerEntity();
			Reattach(Banner, item, dbContext);
			Banner.Id = maxId;
			dbContext.Add(Banner);
		}

		await dbContext.SaveChangesAsync(cancellationToken);

	}


	public virtual async Task Delete(DeleteBannerCommand command, CancellationToken cancellationToken = default)
	{
		if (Computed.IsInvalidating())
		{
			_ = await Invalidate();
			return;
		}
		await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);
		var Banner = await dbContext.Banners
		.Include(x => x.Photo)
		.Where(x => x.Id == command.Id)
		.ToListAsync(cancellationToken) ?? throw new ValidationException("BannerEntity Not Found");

		dbContext.RemoveRange(Banner);
		await dbContext.SaveChangesAsync(cancellationToken);
	}


	public virtual async Task Update(UpdateBannerCommand command, CancellationToken cancellationToken = default)
	{
		if (Computed.IsInvalidating())
		{
			_ = await Invalidate();
			return;
		}
		await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);



		foreach (var item in command.Entity)
		{
			var Banner = dbContext.Banners
			.Include(x => x.Photo)
			.First(x => x.Id == item.Id && x.Locale == item.Locale);

			Reattach(Banner, item, dbContext);

			dbContext.Update(Banner);
		}


		await dbContext.SaveChangesAsync(cancellationToken);
	}
	#endregion



	#region Helpers

	//[ComputeMethod]
	public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
	private void Reattach(BannerEntity Banner, BannerView BannerView, AppDbContext dbContext)
	{
		BannerMapper.From(BannerView, Banner);
		/*if (Banner.Photo != null)
			Banner.Photo = dbContext.Files
			.First(x => x.Id == Banner.Photo.Id);*/

	}

	private void Sorting(ref IQueryable<BannerEntity> Banner, TableOptions options) => Banner = options.SortLabel switch
	{
		"Title" => Banner.Ordering(options, o => o.Title),
		"Locale" => Banner.Ordering(options, o => o.Locale),
		"Link" => Banner.Ordering(options, o => o.Link),
		"Photo" => Banner.Ordering(options, o => o.Photo),
		"Id" => Banner.Ordering(options, o => o.Id),
		_ => Banner.OrderBy(o => o.Id),

	};
	#endregion
}
