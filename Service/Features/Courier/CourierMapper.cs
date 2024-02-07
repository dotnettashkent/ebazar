using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features.Courier
{
    [Mapper]
    public static partial class CourierMapper
    {
        #region Usable
        public static CourierView MapToView(this CourierEntity src) => src.To();
        public static List<CourierView> MapToViewList(this List<CourierEntity> src) => src.ToList();
        public static CourierEntity MapFromView(this CourierView src) => src.From();
        #endregion

        #region Internal

        private static partial CourierView To(this CourierEntity src);
        private static partial List<CourierView> ToList(this List<CourierEntity> src);
        private static partial CourierEntity From(this CourierView BannerView);
        public static partial void From(CourierView courierView, CourierEntity courierEntity);
        #endregion
    }
}
