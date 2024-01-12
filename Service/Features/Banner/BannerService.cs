using Stl.Async;
using Stl.Fusion;
using Service.Data;
using System.Reactive;
using Shared.Features;
using Shared.Infrastructures;
using Stl.Fusion.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructures.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Service.Features;

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
	public async virtual Task<BannerView> Get(long Id, CancellationToken cancellationToken = default)
	{
		var dbContext = _dbHub.CreateDbContext();
		await using var _ = dbContext.ConfigureAwait(false);
		var Banner = await dbContext.Banners
		.FirstOrDefaultAsync(x => x.Id == Id);

		return Banner == null ? throw new ValidationException("BannerEntity Not Found") : Banner.MapToView();
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
        BannerEntity category = new BannerEntity();
        Reattach(category, command.Entity, dbContext);

        dbContext.Update(category);
        await dbContext.SaveChangesAsync();

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
