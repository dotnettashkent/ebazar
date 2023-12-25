using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stl.Fusion.Authentication.Services;
using Stl.Fusion.EntityFramework.Operations;
using Stl.Fusion.EntityFramework;
using Stl.Fusion.Extensions.Services;
using System.Security.Claims;
using EF.Audit.Core;
using Shared.Infrastructures;
using EF.Audit.Core.Extensions;

namespace Service.Data
{
	public partial class AppDbContext : DbContextBase
	{
		private readonly AuditDbContext _context;
		public IServiceScopeFactory _serviceScopeFactory;

		[ActivatorUtilitiesConstructor]
		public AppDbContext(DbContextOptions<AppDbContext> options, IDbContextFactory<AuditDbContext> context,
		  IServiceScopeFactory serviceScopeFactory) : base(options)
		{
			_serviceScopeFactory = serviceScopeFactory;
			_context = context.CreateDbContext();
		}
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
				
        }

        // Stl.Fusion.EntityFramework tables
        public DbSet<DbUser<string>> Users { get; protected set; } = null!;
		public DbSet<DbUserIdentity<string>> UserIdentities { get; protected set; } = null!;
		public DbSet<DbSessionInfo<string>> Sessions { get; protected set; } = null!;
		public DbSet<DbKeyValue> KeyValues { get; protected set; } = null!;
		public DbSet<DbOperation> Operations { get; protected set; } = null!;

		public override int SaveChanges()
		{
			AddTimestamps();
			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			AddTimestamps();

			using (var scope = _serviceScopeFactory.CreateScope())
			{
				var userContext = scope.ServiceProvider.GetService<UserContext>();
				if (userContext!.UserClaims.Count() > 1)
				{
					var identity = userContext.UserClaims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
					await _context.SaveChangesAndAuditAsync(this.ChangeTracker.Entries(), identity, cancellationToken: cancellationToken);
					return await base.SaveChangesAsync(cancellationToken);
				}
				return await base.SaveChangesAsync(cancellationToken);
			}
		}

		private void AddTimestamps()
		{

		}
	}
}
