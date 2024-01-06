using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
    [Table("products")]
    public class ProductEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("locale")]
        public string Locale { get; set; } = null!;

        [Column("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// split ('|') each description
		/// </summary>
        [Column("description")]
		public string Description { get; set; } = null!;

        [Column("brand_name")]
        public string BrandName { get; set; } = null!;

        [Column("count")]
        public int Count { get; set; }

        [Column("max_count")]
        public int MaxCount { get; set; }

        [Column("price")]
        public decimal Price { get; set; }
        
        [Column("discount_price")]
        public decimal DiscountPrice { get; set; }

		[Column("discount_percent")]
		public decimal DiscountPercent { get; set; }

		[Column("price_type")]
        public string PriceType { get; set; } = null!;

        [Column("is_delivery_free")]
        public bool IsDeliveryFree { get; set; }

        [Column("photo")]
        public string Photo { get; set; } = null!;

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
        
        //Relations
        public virtual CartEntity? Cart { get; set; }
        public virtual ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
        public virtual FavouriteEntity? Favourite { get; set; }
    }
}
