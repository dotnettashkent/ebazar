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

		
		private static partial BrandView To(this BrandEntity src);
		
		private static partial List<BrandView> ToList(this List<BrandEntity> src);
		
		private static partial BrandEntity From(this BrandView BannerView);
		
		public static partial void From(BrandView personView, BrandEntity personEntity);
		#endregion
	}
}
