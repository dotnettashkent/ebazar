﻿using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features
{
	[Mapper]
	public static partial class UserMapper
	{
		#region Usable
		public static UserView MapToView(this UserEntity src) => src.To();
		public static List<UserView> MapToViewList(this List<UserEntity> src) => src.ToList();
		public static UserEntity MapFromView(this UserView src) => src.From();
		#endregion

		#region Internal

		[MapProperty("Cart", "CartView")]
		[MapProperty("Orders", "OrdersView")]
		[MapProperty("Favourites", "FavouritesView")]
		private static partial UserView To(this UserEntity src);

		[MapProperty("Cart", "CartView")]
		[MapProperty("Orders", "OrdersView")]
		[MapProperty("Favourites", "FavouritesView")]
		private static partial List<UserView> ToList(this List<UserEntity> src);

		[MapProperty("CartView", "Cart")]
		[MapProperty("OrdersView", "Orders")]
		[MapProperty("FavouritesView", "Favourites")]
		private static partial UserEntity From(this UserView FileView);

		[MapProperty("CartView", "Cart")]
		[MapProperty("OrdersView", "Orders")]
		[MapProperty("FavouritesView", "Favourites")]
		public static partial void From(UserView userView, UserEntity userEntity);
		#endregion
	}
}
