/*using Microsoft.EntityFrameworkCore;
using Stl.Fusion;
using Stl.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using Stl.Async;
using System.Reactive;
using Service.Data;
using Shared.Infrastructures.Extensions;
using Shared;
using Shared.Infrastructures;
using Service.Features;
namespace UtcNew.Services;

public class LocaleService : ILocaleService 
{
    #region Initialize
    private readonly DbHub<AppDbContext> _dbHub;
    
    public LocaleService(DbHub<AppDbContext> dbHub)
    {
        _dbHub = dbHub;
    }
    #endregion
    #region Queries
    //[ComputeMethod]
    public async virtual Task<TableResponse<LocaleView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
    {
        await Invalidate();
        var dbContext = _dbHub.CreateDbContext();
        await using var _ = dbContext.ConfigureAwait(false);
        var locale = from s in dbContext.Locales select s;

        if (!String.IsNullOrEmpty(options.Search))
        {
            locale = locale.Where(s => 
                     s.Code.Contains(options.Search)
            );
        }

        Sorting(ref locale, options);
        
        locale = locale.Include(x => x.ProductEntity);
        var count = await locale.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
        var items = await locale.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
        return new TableResponse<LocaleView>(){ Items = items.MapToViewList(), TotalItems = count };
    }

    //[ComputeMethod]
    public async virtual Task<LocaleView> Get(string Code, CancellationToken cancellationToken = default)
    {
        var dbContext = _dbHub.CreateDbContext();
        await using var _ = dbContext.ConfigureAwait(false);
        var locale = await dbContext.Locales
        .Include(x => x.ProductEntity)
        .FirstOrDefaultAsync(x => x.Code == Code);
        
        return locale == null ? throw new ValidationException("LocaleEntity Not Found") : locale.MapToView();
    }

    #endregion
    #region Mutations
    public async virtual Task Create(CreateLocaleCommand command, CancellationToken cancellationToken = default)
    {
        if (Computed.IsInvalidating())
        {
            _ = await Invalidate();
            return;
        }

        await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);
        LocaleEntity locale=new LocaleEntity();
        Reattach(locale, command.Entity, dbContext); 
        
        dbContext.Update(locale);
        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async virtual Task Delete(DeleteLocaleCommand command, CancellationToken cancellationToken = default)
    {
        if (Computed.IsInvalidating())
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);
        var locale = await dbContext.Locales
        .Include(x=>x.ProductEntity)
        .FirstOrDefaultAsync(x => x.Code == command.Code);
        if (locale == null) throw  new ValidationException("LocaleEntity Not Found");
        dbContext.Remove(locale);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public async virtual Task Update(UpdateLocaleCommand command, CancellationToken cancellationToken = default)
    {
        if (Computed.IsInvalidating())
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);
        var locale = await dbContext.Locales
        .Include(x=>x.ProductEntity)
        .FirstOrDefaultAsync(x => x.Code == command.Entity!.Code);

        if (locale == null) throw  new ValidationException("LocaleEntity Not Found"); 

        Reattach(locale, command.Entity, dbContext);
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
    #endregion

    

    #region Helpers

    //[ComputeMethod]
    public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
    private void Reattach(LocaleEntity locale, LocaleView localeView, AppDbContext dbContext)
    {
        LocaleMapper.From(localeView, locale);


        if(locale.ProductEntity != null)
        locale.ProductEntity  = dbContext.Products
        .Where(x => locale.ProductEntity.Select(tt => tt.Id).ToList().Contains(x.Id)).ToList();
    }

    private void Sorting(ref IQueryable<LocaleEntity> locale, TableOptions options) => locale = options.SortLabel switch
    {
        "Code" => locale.Ordering(options, o => o.Code),
        "ProductEntity" => locale.Ordering(options, o => o.ProductEntity),
        _ => locale.OrderBy(o => o.Code),
        
    };
    #endregion
}
*/