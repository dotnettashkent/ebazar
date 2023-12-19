using Microsoft.EntityFrameworkCore;
using Shared.Features;
using Shared.Features.Brands;
using Shared.Features.Favourite;
using Shared.Features.File;

namespace Service.Data
{
	public partial class AppDbContext
	{
		public virtual DbSet<AddressEntity> Addresses { get; set; }
		public virtual DbSet<BannerEntity> Banners { get; set; }
		public virtual DbSet<BrandEntity> Brands { get; set; }
		public virtual DbSet<CartEntity> Carts { get; set; }
		public virtual DbSet<CourierEntity> Couriers { get; set; }
		public virtual DbSet<FavouriteEntity> Favorites { get; set; }
		public virtual DbSet<FileEntity> Files { get; set; }
		public virtual DbSet<OrderEntity> Orders { get; set; }
		public virtual DbSet<ProductEntity> Products { get; set; }
		public virtual DbSet<ProductCategoryEntity> ProductCategories { get; set; }
		public virtual DbSet<UserEntity> UsersEn { get; set; }
	}
}
