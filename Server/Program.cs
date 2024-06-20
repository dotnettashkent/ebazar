using Microsoft.EntityFrameworkCore;
using Server.Infrastructure.ServiceCollection;
using Service.Data;
using Stl.Fusion;
using DotNetEnv;

Env.Load("../.env");

#region Builder
var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var cfg = builder.Configuration;
var env = builder.Environment;
#endregion

#region Database
var dbType = cfg.GetValue<string>("DatabaseProviderConfiguration:ProviderType");
var connectionString = $"Host={cfg["POSTGRESS_HOST"]};" +
                       $"Port={cfg["POSTGRESS_PORT"]};" +
                       $"User Id={cfg["POSTGRESS_USER"]};" +
                       $"Database={cfg["POSTGRESS_DBNAME"]};" +
                       $"Password={cfg["POSTGRESS_PASSWORD"]};";

services.AddDataBase<AppDbContext>(env, cfg, (DataBaseType)Enum.Parse(typeof(DataBaseType), dbType, true), connectionString);

// Register IDbContextFactory<AuditDbContext> before AddDataBase<AppDbContext>
services.AddDbContext<AppDbContext>(options =>
{
    // Configure options for AuditDbContext
    options.UseNpgsql(connectionString);
});
#endregion

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

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
    app.UseDeveloperExceptionPage();
    app.UseOpenApi();
    app.UseSwaggerUi3();
}
/*else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}*/

app.UseHttpsRedirection();
app.UseCors();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllers();
app.Run();
