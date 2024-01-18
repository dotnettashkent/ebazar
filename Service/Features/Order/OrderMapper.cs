using Shared.Features;
using Riok.Mapperly.Abstractions;

namespace Service.Features.Order
{
    [Mapper]
    public static partial class OrderMapper
    {
        #region Usable
        public static OrderView MapToView(this OrderEntity src) => src.To();
        public static List<OrderView> MapToViewList(this List<OrderEntity> src) => src.ToList();
        public static OrderEntity MapFromView(this OrderView src) => src.From();
        #endregion

        #region Internal

        [MapProperty("CartEntity","CartView")]
        [MapProperty("UserEntity","UserView")]
        private static partial OrderView To(this OrderEntity src);

        [MapProperty("CartEntity", "CartView")]
        [MapProperty("UserEntity", "UserView")]
        private static partial List<OrderView> ToList(this List<OrderEntity> src);

        [MapProperty("CartView", "CartEntity")]
        [MapProperty("UserView", "UserEntity")]
        private static partial OrderEntity From(this OrderView view);
        
        [MapProperty("CartView", "CartEntity")]
        [MapProperty("UserView", "UserEntity")]
        public static partial void From(OrderView view, OrderEntity entity);
        #endregion
    }
}
