using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features.ProductCategory
{
	[Mapper]
	public static partial class ProductCategoryMapper
	{
		public static ProductCategoryView MapToView(this ProductCategoryEntity src) => src.To();
		public static List<ProductCategoryView> MapToViewList(this List<ProductCategoryEntity> src) => src.ToList();
		public static ProductCategoryEntity MapFromView(this ProductCategoryView src) => src.From();


		/*[MapProperty("Photo", "PhotoView")]*/
		private static partial ProductCategoryView To(this ProductCategoryEntity src);
		/*[MapProperty("Photo", "PhotoView")]*/
		private static partial List<ProductCategoryView> ToList(this List<ProductCategoryEntity> src);
		/*[MapProperty("PhotoView", "Photo")]*/
		private static partial ProductCategoryEntity From(this ProductCategoryView ProductCategoryView);
		/*[MapProperty("PhotoView", "Photo")]*/
		public static partial void From(ProductCategoryView personView, ProductCategoryEntity personEntity);

	}
}
