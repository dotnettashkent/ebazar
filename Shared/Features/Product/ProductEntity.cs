using Shared.Infrastructures;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features.Product
{
    public class ProductEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("name_en")]
        public string NameEn { get; set; } = null!;

        [Column("name_ru")]
        public string NameRu { get; set; } = null!;

        [Column("name_uz")]
        public string NameUz { get; set; } = null!;

        [Column("description_en")]
        public string DescriptionEn { get; set; } = null!;

        [Column("description_ru")]
        public string DescriptionRu { get; set; } = null!;

        [Column("description_uz")]
        public string DescriptionUz { get; set; } = null!;

        [Column("count")]
        public int Count { get; set; }

        [Column("max_count")]
        public int MaxCount { get; set; }

        [Column("price")]
        public decimal Price { get; set; }
        
        [Column("discount_price")]
        public decimal DiscountPrice { get; set; }

        [Column("unit_type")]
        public UnitType UnitType { get; set; }

        [Column("is_free_delivery")]
        public bool IsFreeDelivery { get; set; } = false;

        [Column("delivery_time")]
        public DateTime DeliveryTime { get; set; }


        [Column("photo_id")]
        public long PhotoId { get; set; }

        [Column("tag")]
        public string? Tag {  get; set; }

        [Column("weight")]
        public decimal Weight { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("is_popular")]
        public bool IsPopular { get; set; }

        [Column("is_holiday")]
        public bool IsHoliday { get; set; }

        [Column("is_big_sale")]
        public bool IsBigSale { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set;} = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
