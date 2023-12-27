using Microsoft.EntityFrameworkCore;
using Service.Data;
using Stl.Fusion;
using Server.Infrastructure.ServiceCollection;
using EF.Audit.Core;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var cfg = builder.Configuration;
var env = builder.Environment;

// Database
var dbType = cfg.GetValue<string>("DatabaseProviderConfiguration:ProviderType");
services.AddDataBase<AppDbContext>(env, cfg, (DataBaseType)Enum.Parse(typeof(DataBaseType), dbType, true));

// Register IDbContextFactory<AuditDbContext> before AddDataBase<AppDbContext>
services.AddDbContextFactory<AuditDbContext>(options =>
{
	// Configure options for AuditDbContext
	options.UseNpgsql(cfg.GetConnectionString("Default"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseNpgsql(cfg.GetConnectionString("Default")));

// STL.Fusion
IComputedState.DefaultOptions.MustFlowExecutionContext = true;
builder.Services.AddFusionServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseOpenApi();
	app.UseSwaggerUi3();
}
else
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
