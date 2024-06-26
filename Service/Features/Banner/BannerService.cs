using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service.Data;
using Shared.Features;
using Stl.Async;
using Stl.Fusion;
using System.Reactive;
using Shared.Infrastructures;
using Stl.Fusion.EntityFramework;
using System.IdentityModel.Tokens.Jwt;
using Shared.Infrastructures.Extensions;

namespace Service.Features;

public class BannerService : IBannerService
{
    #region Initialize
    private readonly DbHub<AppDbContext> _dbHub;
    private readonly IFileService fileService;
    private readonly IConfiguration configuration;
    public BannerService(DbHub<AppDbContext> dbHub, IFileService fileService, IConfiguration configuration)
    {
        _dbHub = dbHub;
        this.fileService = fileService;
        this.configuration = configuration;
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
        command.Session.IsDefault();
        var phoneNumber = ValidateToken(command.Token);
        if (!IsAdminUser(phoneNumber))
        {
            throw new CustomException("Not Permission");
        }
        if (Computed.IsInvalidating())
        {
            _ = await Invalidate();
            return;
        }
        if (command.Entity.PhotoView != null)
        {
            var fileResult = await fileService.UploadMediaAsync(command.Entity.PhotoView);
            command.Entity.Photo = fileResult;
        }

        await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);
        BannerEntity category = new BannerEntity();
        Reattach(category, command.Entity, dbContext);

        dbContext.Update(category);
        await dbContext.SaveChangesAsync();

    }


    public virtual async Task Delete(DeleteBannerCommand command, CancellationToken cancellationToken = default)
    {
        command.Session.IsDefault();
        var phoneNumber = ValidateToken(command.Token);
        if (!IsAdminUser(phoneNumber))
        {
            throw new CustomException("Not Permission");
        }
        if (Computed.IsInvalidating())
        {
            _ = await Invalidate();
            return;
        }
        await using var dbContext = await _dbHub.CreateCommandDbContext(cancellationToken);
        var Banner = await dbContext.Banners
        .FirstOrDefaultAsync(x => x.Id == command.Id) ?? throw new CustomException("BannerEntity Not Found");
        await fileService.DeleteMediaAsync(Banner.Photo);
        dbContext.RemoveRange(Banner);
        await dbContext.SaveChangesAsync(cancellationToken);
    }


    public virtual async Task Update(UpdateBannerCommand command, CancellationToken cancellationToken = default)
    {
        command.Session.IsDefault();
        var phoneNumber = ValidateToken(command.Token);
        if (!IsAdminUser(phoneNumber))
        {
            throw new CustomException("Not Permission");

        }
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
        "link" => Banner.Ordering(options, o => o.Link),
        "id" => Banner.Ordering(options, o => o.Id),
        _ => Banner.OrderBy(o => o.Id),

    };
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
}
