using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features
{
	[Mapper]
	public static partial class UserMapper
	{
		#region Usable
		public static UserView MapToView(this UserEntity src) => src.To();
        public static UserResultView MapToResultView(this UserEntity src) => src.ToResult();
        public static List<UserView> MapToViewList(this List<UserEntity> src) => src.ToList();
		public static UserEntity MapFromView(this UserView src) => src.From();
        public static UserEntity MapFromResultView(this UserResultView src) => src.FromResult();
        #endregion

        #region Internal

        [MapProperty("Cart", "CartView")]
		[MapProperty("Orders", "OrdersView")]
		[MapProperty("Favourite", "FavouritesView")]
		private static partial UserView To(this UserEntity src);

        private static partial UserResultView ToResult(this UserEntity src);

        [MapProperty("Cart", "CartView")]
		[MapProperty("Orders", "OrdersView")]
		[MapProperty("Favourites", "FavouritesView")]
		private static partial List<UserView> ToList(this List<UserEntity> src);

		[MapProperty("CartView", "Cart")]
		[MapProperty("OrdersView", "Orders")]
		[MapProperty("FavouritesView", "Favourite")]
		private static partial UserEntity From(this UserView FileView);

		[MapProperty("CartView", "Cart")]
		[MapProperty("OrdersView", "Orders")]
		[MapProperty("FavouritesView", "Favourite")]
		public static partial void From(UserView userView, UserEntity userEntity);


        
        private static partial UserEntity FromResult(this UserResultView FileView);
		public static partial void FromResult(UserResultView userView, UserEntity userEntity);

        #endregion
    }
}
