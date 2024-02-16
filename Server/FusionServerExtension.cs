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
            fusion.AddService<IAddressService, AddressService>();
            fusion.AddService<IBannerService, BannerService>();
            fusion.AddService<IBrandService, BrandService>();
            fusion.AddService<ICartService, CartService>();
            fusion.AddService<ICourierService, CourierService>();
            fusion.AddService<IFavouriteService, FavouriteService>();
            fusion.AddService<IProductService, ProductService>();
            fusion.AddService<IUserService, UserService>();
            fusion.AddService<IOrderServices, OrderService>();
            fusion.AddService<IFileService, FileService>();
            fusion.AddService<IProductCategoryService, ProductCategoryService>();
            fusion.AddService<IProductSubCategoryService, ProductSubCategoryService>();
            //fusion.AddService<ILocaleService, LocaleService>();
            return fusion;
        }
    }
}
