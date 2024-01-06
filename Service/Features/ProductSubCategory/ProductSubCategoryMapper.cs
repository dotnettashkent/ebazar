using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features
{
	[Mapper]
	public static partial class ProductSubCategoryMapper
	{
		#region Usable
		public static ProductSubCategoryView MapToView(this ProductSubCategoryEntity src) => src.To();
		public static List<ProductSubCategoryView> MapToViewList(this List<ProductSubCategoryEntity> src) => src.ToList();
		public static ProductSubCategoryEntity MapFromView(this ProductSubCategoryView src) => src.From();
		#endregion


		#region Internal

		private static partial ProductSubCategoryView To(this ProductSubCategoryEntity src);

		private static partial List<ProductSubCategoryView> ToList(this List<ProductSubCategoryEntity> src);

		private static partial ProductSubCategoryEntity From(this ProductSubCategoryView ProductSubCategoryView);

		public static partial void From(ProductSubCategoryView personView, ProductSubCategoryEntity personEntity);
		#endregion
	}
}
