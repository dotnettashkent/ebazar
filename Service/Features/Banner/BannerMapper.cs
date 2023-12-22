using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features.Banner
{
	[Mapper]
	public static partial class BannerMapper
	{
		#region Usable
		public static BannerView MapToView(this BannerEntity src) => src.To();
		public static List<BannerView> MapToViewList(this List<BannerEntity> src) => src.ToList();
		public static BannerEntity MapFromView(this BannerView src) => src.From();
		#endregion

		#region Internal
		[MapProperty("Photo", "PhotoView")]
		private static partial BannerView To(this BannerEntity src);
		[MapProperty("Photo", "PhotoView")]
		private static partial List<BannerView> ToList(this List<BannerEntity> src);
		[MapProperty("PhotoView", "Photo")]
		private static partial BannerEntity From(this BannerView BannerView);
		[MapProperty("PhotoView", "Photo")]
		public static partial void From(BannerView personView, BannerEntity personEntity);
		#endregion
	}
}
