using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features.Cart
{
	[Mapper]
	public static partial class CartMapper
	{
		#region Usable
		public static CartView MapToView(this CartEntity src) => src.To();
		public static List<CartView> MapToViewList(this List<CartEntity> src) => src.ToList();
		public static CartEntity MapFromView(this CartView src) => src.From();
		#endregion

		#region Internal

		[MapProperty("Products", "ProductsView")]
		[MapProperty("User","UserView")]
		private static partial CartView To(this CartEntity src);

		[MapProperty("Products", "ProductsView")]
		[MapProperty("User", "UserView")]
		private static partial List<CartView> ToList(this List<CartEntity> src);

		[MapProperty("ProductsView", "Products")]
		[MapProperty("UserView", "User")]
		private static partial CartEntity From(this CartView view);

		[MapProperty("ProductsView", "Products")]
		[MapProperty("UserView", "User")]
		public static partial void From(CartView personView, CartEntity personEntity);
		#endregion
	}
}
