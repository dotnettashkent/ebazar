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
		//public virtual DbSet<LocaleEntity> Locales { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

			/*modelBuilder.Entity<AddressEntity>(entity =>
			{
				entity.HasKey(x => x.Id).HasName("address_pkey");
				entity.HasOne(x => x.User).WithMany(x => x.Addresses)
				.HasForeignKey(x => x.UserId)
				.HasConstraintName("address_id_fkey");
            });

			modelBuilder.Entity<CartEntity>(entity =>
			{
				entity.HasOne(c => c.User)
				.WithOne(u => u.Cart)
				.HasForeignKey<CartEntity>(c => c.UserId);
			});

            modelBuilder.Entity<FavouriteEntity>(entity =>
            {
				entity.HasOne(c => c.User)
				.WithOne(u => u.Favourite);
            });



            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.HasKey(x => x.Id).HasName("product_pkey");

				entity.HasOne(x => x.Cart)
				.WithMany(c => c.Products)
				.HasForeignKey(x => x.CartId)
				.HasConstraintName("cart_id_fkey");

                entity.HasOne(x => x.Favourite)
                .WithMany(c => c.ProductEntity)
                .HasForeignKey(x => x.FavouriteId)
                .HasConstraintName("favourite_id_fkey");

            });*/

			/*modelBuilder.Entity<UserEntity>(entity =>
			{
				entity.HasKey(x => x.Id).HasName("user_pkey");

				entity.HasOne(u => u.Cart)
				.WithOne(c => c.User)
				.HasForeignKey<CartEntity>(c => c.UserId);

				entity.HasMany(u => u.Orders)
					.WithOne(o => o.User)
					.HasForeignKey(o => o.UserId)
					.OnDelete(DeleteBehavior.Cascade); // Adjust delete behavior as needed

				entity.HasMany(u => u.Addresses)
					.WithOne(a => a.User)
					.HasForeignKey(a => a.UserId)
					.OnDelete(DeleteBehavior.Cascade);
			});*/

			base.OnModelCreating(modelBuilder);
        }
	}
}
