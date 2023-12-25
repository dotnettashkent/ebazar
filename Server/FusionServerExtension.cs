using Service.Features;
using Shared.Features;
using Stl.Fusion;

namespace Server
{
    public static class FusionServerExtension
    {
        public static FusionBuilder AddEbazarServices(this FusionBuilder fusion)
        {
            fusion.AddService<IProductCategoryService, ProductCategoryService>();
            fusion.AddService<IFileService, FileService>();
            return fusion;
        }
    }
}
