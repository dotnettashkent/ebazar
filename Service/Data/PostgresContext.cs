using Shared.Features;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Service.Data
{
	public partial class AppDbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
		public virtual DbSet<AddressEntity> Addresses { get; set; }
		public virtual DbSet<BannerEntity> Banners { get; set; }
		public virtual DbSet<BrandEntity> Brands { get; set; }
		public virtual DbSet<CartEntity> Carts { get; set; }
		public virtual DbSet<CourierEntity> Couriers { get; set; }
		public virtual DbSet<FavouriteEntity> Favorites { get; set; }
		public virtual DbSet<FileEntity> Files { get; set; }
		public virtual DbSet<OrderEntity> Orders { get; set; }
		public virtual DbSet<ProductEntity> Products { get; set; }
		/*public virtual DbSet<ProductCategoryEntity> ProductCategories { get; set; }
		public virtual DbSet<ProductSubCategoryEntity> ProductSubCategories { get; set; }*/
		public virtual DbSet<UserEntity> UsersEntities { get; set; }
		public virtual DbSet<LocaleEntity> Locales { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
            
        }
	}
}
