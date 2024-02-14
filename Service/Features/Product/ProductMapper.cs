using Riok.Mapperly.Abstractions;
using Shared.Features;

namespace Service.Features
{
    [Mapper]
    public static partial class ProductMapper
    {
        #region Usable
        public static ProductResultView MapToView(this ProductEntity src) => src.To2();
        public static ProductView MapToView2(this ProductEntity src) => src.To();
        public static List<ProductView> MapToViewList(this List<ProductEntity> src) => src.ToList();
        public static List<ProductResultView> MapToViewListResult(this List<ProductEntity> src) => src.ToListResult();
        public static ProductEntity MapFromView(this ProductView src) => src.From();
        public static ProductEntity MapFromResult(this ProductResultView src) => src.From2();

        #endregion


        #region Internal

        private static partial ProductView To(this ProductEntity src);
        private static partial ProductResultView To2(this ProductEntity src);
        private static partial List<ProductView> ToList(this List<ProductEntity> src);
        private static partial List<ProductResultView> ToListResult(this List<ProductEntity> src);
        private static partial ProductEntity From(this ProductView ProductCategoryView);
        public static partial void From(ProductView personView, ProductEntity personEntity);
        private static partial ProductEntity From2(this ProductResultView ProductCategoryView);
        public static partial void From2(ProductResultView personView, ProductEntity personEntity);
        #endregion
    }
}
