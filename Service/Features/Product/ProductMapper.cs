using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features
{
	[Mapper]
	public static partial class ProductMapper
	{
		#region Usable
		public static ProductView MapToView(this ProductEntity src) => src.To();
		public static List<ProductView> MapToViewList(this List<ProductEntity> src) => src.ToList();
		public static ProductEntity MapFromView(this ProductView src) => src.From();

		#endregion


		#region Internal

		/*[MapProperty("Photo", "PhotoView")]
		[MapProperty("PhotoMobile", "PhotoMobileView")]*/
		private static partial ProductView To(this ProductEntity src);
		/*[MapProperty("Photo", "PhotoView")]
		[MapProperty("PhotoMobile", "PhotoMobileView")]*/
		private static partial List<ProductView> ToList(this List<ProductEntity> src);

        /*[MapProperty("PhotoView", "Photo")]
		[MapProperty("PhotoMobileView", "PhotoMobile")]*/
        private static partial ProductEntity From(this ProductView ProductCategoryView);
		/*[MapProperty("PhotoView", "Photo")]
		[MapProperty("PhotoMobileView", "PhotoMobile")]*/
		public static partial void From(ProductView personView, ProductEntity personEntity);
		#endregion
	}
}
