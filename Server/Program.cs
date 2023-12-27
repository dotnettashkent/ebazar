using Stl.Fusion;
using Service.Data;
using EF.Audit.Core;
using Microsoft.EntityFrameworkCore;
using Server.Infrastructure.ServiceCollection;

#region Builder
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var cfg = builder.Configuration;
var env = builder.Environment;
#endregion

#region Database
var dbType = cfg.GetValue<string>("DatabaseProviderConfiguration:ProviderType");
services.AddDataBase<AppDbContext>(env, cfg, (DataBaseType)Enum.Parse(typeof(DataBaseType), dbType, true));

// Register IDbContextFactory<AuditDbContext> before AddDataBase<AppDbContext>
services.AddDbContextFactory<AuditDbContext>(options =>
{
	// Configure options for AuditDbContext
	options.UseNpgsql(cfg.GetConnectionString("Default"));
});
#endregion

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();

#region STL.Fusion
IComputedState.DefaultOptions.MustFlowExecutionContext = true;
builder.Services.AddFusionServices();
#endregion

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
