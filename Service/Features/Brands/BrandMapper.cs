using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features
{
	[Mapper]
	public static partial class BrandMapper
	{
		#region Usable
		public static BrandView MapToView(this BrandEntity src) => src.To();
		public static List<BrandView> MapToViewList(this List<BrandEntity> src) => src.ToList();
		public static BrandEntity MapFromView(this BrandView src) => src.From();
		#endregion

		#region Internal

		[MapProperty("Photo", "PhotoView")]
		private static partial BrandView To(this BrandEntity src);
		[MapProperty("Photo", "PhotoView")]
		private static partial List<BrandView> ToList(this List<BrandEntity> src);
		[MapProperty("PhotoView", "Photo")]
		private static partial BrandEntity From(this BrandView BannerView);
		[MapProperty("PhotoView", "Photo")]
		public static partial void From(BrandView personView, BrandEntity personEntity);
		#endregion
	}
}
