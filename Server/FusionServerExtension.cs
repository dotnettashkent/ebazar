using Shared.Features.Banner;
using Stl.Fusion;

namespace Server
{
    public static class FusionServerExtension
    {
        public static FusionBuilder AddEbazarServices(this FusionBuilder fusion)
        {
            fusion.AddService<IBannerService>();
//            fusion.AddService<IFileService>();
            return fusion;
        }
    }
}
