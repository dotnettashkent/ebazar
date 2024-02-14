﻿using Microsoft.EntityFrameworkCore;
using Service.Data;
using Shared.Features;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.Fusion;
using Stl.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Reactive;

namespace Service.Features;

public class BannerService : IBannerService
{
    #region Initialize
    private readonly DbHub<AppDbContext> _dbHub;
    private readonly IFileService fileService;

    public BannerService(DbHub<AppDbContext> dbHub, IFileService fileService)
    {
        _dbHub = dbHub;
        this.fileService = fileService;
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

        if (!String.IsNullOrEmpty(options.search))
        {
            Banner = Banner.Where(s =>
                     s.Title.Contains(options.search)
                    || s.Description.Contains(options.search)
            );
        }

        Sorting(ref Banner, options);

        var count = await Banner.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await Banner.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);

        decimal totalPage = (decimal)count / (decimal)options.page_size;

        return new TableResponse<BannerView>() { Items = items.MapToViewList(), TotalItems = count, AllPage = (int)Math.Ceiling(totalPage), CurrentPage = options.page };
    }

    //[ComputeMethod]
    public async virtual Task<BannerView> Get(long Id, CancellationToken cancellationToken = default)
    {
        var dbContext = _dbHub.CreateDbContext();
        await using var _ = dbContext.ConfigureAwait(false);
        var Banner = await dbContext.Banners
        .FirstOrDefaultAsync(x => x.Id == Id);

        return Banner == null ? throw new CustomException("BannerEntity Not Found") : Banner.MapToView();
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
        if (command.Entity.PhotoView != null)
        {
            var fileResult = await fileService.SaveImage(command.Entity.PhotoView);
            if (fileResult.Item1 == 1)
            {
                command.Entity.Photo = fileResult.Item2;
            }
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
        .FirstOrDefaultAsync(x => x.Id == command.Id) ?? throw new CustomException("BannerEntity Not Found");
        await fileService.DeleteImage(Banner.Photo);
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
        var brand = await dbContext.Banners
        .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

        if (brand == null) throw new CustomException("BannerEntity Not Found");

        Reattach(brand, command.Entity, dbContext);
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

    private void Sorting(ref IQueryable<BannerEntity> Banner, TableOptions options) => Banner = options.sort_label switch
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
