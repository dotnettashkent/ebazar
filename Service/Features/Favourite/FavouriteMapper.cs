using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features
{
	[Mapper]
	public static partial class FavouriteMapper
	{
		#region Usable
		public static FavouriteView MapToView(this FavouriteEntity src) => src.To();
		public static List<FavouriteView> MapToViewList(this List<FavouriteEntity> src) => src.ToList();
		public static FavouriteEntity MapFromView(this FavouriteView src) => src.From();
		#endregion

		#region Internal

		[MapProperty("Products", "ProductsView")]
		[MapProperty("User", "UserView")]
		private static partial FavouriteView To(this FavouriteEntity src);
		[MapProperty("Products", "ProductsView")]
		[MapProperty("User", "UserView")]
		private static partial List<FavouriteView> ToList(this List<FavouriteEntity> src);
		[MapProperty("ProductsView", "Products")]
		[MapProperty("UserView", "User")]
		private static partial FavouriteEntity From(this FavouriteView BannerView);
		[MapProperty("ProductsView", "Products")]
		[MapProperty("UserView", "User")]
		public static partial void From(FavouriteView personView, FavouriteEntity personEntity);
		#endregion
	}
}
