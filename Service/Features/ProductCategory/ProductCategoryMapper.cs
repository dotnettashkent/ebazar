using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features.ProductCategory
{
	[Mapper]
	public static partial class ProductCategoryMapper
	{
		#region Usable
		public static ProductCategoryView MapToView(this ProductCategoryEntity src) => src.To();
		public static List<ProductCategoryView> MapToViewList(this List<ProductCategoryEntity> src) => src.ToList();
		public static ProductCategoryEntity MapFromView(this ProductCategoryView src) => src.From();
		#endregion


		#region Internal

		private static partial ProductCategoryView To(this ProductCategoryEntity src);
		
		private static partial List<ProductCategoryView> ToList(this List<ProductCategoryEntity> src);
		
		private static partial ProductCategoryEntity From(this ProductCategoryView ProductCategoryView);
		
		public static partial void From(ProductCategoryView personView, ProductCategoryEntity personEntity);
		#endregion

	}
}
