using Shared.Features;
using Riok.Mapperly.Abstractions;

namespace Service.Features.Order
{
    [Mapper]
    public static partial class OrderMapper
    {
        #region Usable
        public static OrderView MapToView(this OrderEntity src) => src.To();
        public static OrderResponse MapToView2(this OrderEntity src) => src.To2();
        public static List<OrderView> MapToViewList(this List<OrderEntity> src) => src.ToList();
        public static OrderEntity MapFromView(this OrderView src) => src.From();
        #endregion

        #region Internal

       
        private static partial OrderView To(this OrderEntity src);
        
        private static partial OrderResponse To2(this OrderEntity src);


        private static partial List<OrderView> ToList(this List<OrderEntity> src);

        
        private static partial OrderEntity From(this OrderView view);
        
        
        public static partial void From(OrderView view, OrderEntity entity);
        #endregion
    }
}
