using Service.Features;
using Service.Features.Courier;
using Service.Features.User;
using Shared.Features;
using Stl.Fusion;

namespace Server
{
	public static class FusionServerExtension
	{
		public static FusionBuilder AddEbazarServices(this FusionBuilder fusion)
		{
			fusion.AddService<IBannerService, BannerService>();
			fusion.AddService<IBrandService, BrandService>();
			fusion.AddService<ICourierService, CourierService>();
			fusion.AddService<IFavouriteService, FavouriteService>();
			fusion.AddService<IFileService, FileService>();
			fusion.AddService<IProductService, ProductService>();
			fusion.AddService<IProductCategoryService, ProductCategoryService>();
			fusion.AddService<IUserService, UserService>();

			return fusion;
		}
	}
}
