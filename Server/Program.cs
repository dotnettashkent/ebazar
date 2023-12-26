using EF.Audit.Core.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using Server.Infrastructure.ServiceCollection;
using Service.Data;
using Stl.Fusion;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var cfg = builder.Configuration;
var env = builder.Environment;

#region STL.Fusion
IComputedState.DefaultOptions.FlowExecutionContext = true;
services.AddFusionServices();
#endregion

#region MudBlazor and Pages
services.AddRazorPages();
services.AddServerSideBlazor(o => o.DetailedErrors = true);
services.AddMudServices(config =>
{
	config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
	config.SnackbarConfiguration.PreventDuplicates = false;
	config.SnackbarConfiguration.NewestOnTop = false;
	config.SnackbarConfiguration.ShowCloseIcon = true;
	config.SnackbarConfiguration.VisibleStateDuration = 10000;
	config.SnackbarConfiguration.HideTransitionDuration = 500;
	config.SnackbarConfiguration.ShowTransitionDuration = 500;
	config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
#endregion

#region Localization
services.AddLocalization();
services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = new[] { "en-US", "ru-RU", "uz-Latn" };
	options.DefaultRequestCulture = new RequestCulture("en-US");
	options.AddSupportedCultures(supportedCultures);
	options.AddSupportedUICultures(supportedCultures);
});
#endregion

#region Audit and HttpAcess
services.AddAudit(cfg);
services.AddHttpContextAccessor();
#endregion





builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();
/*var dbContextFactory = app.Services.GetRequiredService<IDbContextFactory<AppDbContext>>();
using var dbContext = dbContextFactory.CreateDbContext();
await dbContext.Database.MigrateAsync();*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
