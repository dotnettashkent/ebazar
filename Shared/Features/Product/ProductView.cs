using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class ProductView
    {
        [property : DataMember] 
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [property : DataMember] 
        [JsonPropertyName("name_uz")]
        public string? NameUz { get; set; }

        [property : DataMember] 
        [JsonPropertyName("name_ru")]
        public string? NameRu { get; set; }

        [property : DataMember] 
        [JsonPropertyName("description_uz")]
        public string? DescriptionUz { get; set; }

        [property : DataMember] 
        [JsonPropertyName("description_ru")]
        public string? DescriptionRu { get; set; }


        [property : DataMember] 
        [JsonPropertyName("brand_name")]
        public string? BrandName { get; set; }
        
        [property : DataMember]
        [JsonPropertyName("count")]
        public int? Count { get; set; }

        [property : DataMember] 
        [JsonPropertyName("max_count")]
        public int? MaxCount { get; set; }

        [property : DataMember] 
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        [property : DataMember] 
        [JsonPropertyName("discount_price")]
        public decimal? DiscountPrice { get; set; }

        [property : DataMember] 
        [JsonPropertyName("price_type")]
        public string? PriceType { get; set; }

        [property : DataMember] 
        [JsonPropertyName("is_delivery_free")]
        public bool IsFreeDelivery { get; set; } = false;

        [property : DataMember] 
        [JsonPropertyName("photo")]
        public string? Photo { get; set; }

        [property : DataMember] 
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }

        [property : DataMember] 
        [JsonPropertyName("weight")]
        public decimal? Weight { get; set; }

        [property : DataMember] 
        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [property : DataMember] 
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; } = false;

        [property : DataMember] 
        [JsonPropertyName("is_popular")]
        public bool IsPopular { get; set; } = false;

        [property : DataMember] 
        [JsonPropertyName("is_holiday")]
        public bool IsHoliday { get; set; } = false;

        [property : DataMember] 
        [JsonPropertyName("is_big_sale")]
        public bool IsBigSale { get; set; } = false;


		[property : DataMember] 
        [JsonPropertyName("category")]
        public string? Category { get; set; }

		[property : DataMember] 
        [JsonPropertyName("sub_category")]
        public string? SubCategory { get; set; }

        [JsonPropertyName("created_at")]

		[property : DataMember] 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [property : DataMember] 
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        //Relations

        public virtual CartView? CartView { get; set; }
		public virtual ICollection<OrderView> OrdersView { get; set; } = new List<OrderView>();
		public virtual FavouriteView? FavouriteView { get; set; }


		public override bool Equals(object? o)
        {
            var other = o as ProductView;
            return other?.Id == Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }

    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class ProductResultView
    {
        [property: DataMember]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [property: DataMember]
        [JsonPropertyName("name_uz")]
        public string? NameUz { get; set; }

        [property: DataMember]
        [JsonPropertyName("name_ru")]
        public string? NameRu { get; set; }

        [property: DataMember]
        [JsonPropertyName("description_uz")]
        public string? DescriptionUz { get; set; }

        [property: DataMember]
        [JsonPropertyName("description_ru")]
        public string? DescriptionRu { get; set; }

        [property: DataMember]
        [JsonPropertyName("brand_name")]
        public string? BrandName { get; set; }

        [property: DataMember]
        [JsonPropertyName("count")]
        public int? Count { get; set; }

        [property: DataMember]
        [JsonPropertyName("max_count")]
        public int? MaxCount { get; set; }

        [property: DataMember]
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        [property: DataMember]
        [JsonPropertyName("discount_price")]
        public decimal? DiscountPrice { get; set; }

        [property: DataMember]
        [JsonPropertyName("price_type")]
        public string? PriceType { get; set; }

        [property: DataMember]
        [JsonPropertyName("is_delivery_free")]
        public bool IsFreeDelivery { get; set; } = false;

        [property: DataMember]
        [JsonPropertyName("photo")]
        public string? Photo { get; set; }

        [property: DataMember]
        [JsonPropertyName("tag")]
        public string? Tag { get; set; }

        [property: DataMember]
        [JsonPropertyName("weight")]
        public decimal? Weight { get; set; }

        [property: DataMember]
        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [property: DataMember]
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; } = false;

        [property: DataMember]
        [JsonPropertyName("is_popular")]
        public bool IsPopular { get; set; } = false;

        [property: DataMember]
        [JsonPropertyName("is_holiday")]
        public bool IsHoliday { get; set; } = false;

        [property: DataMember]
        [JsonPropertyName("is_big_sale")]
        public bool IsBigSale { get; set; } = false;


        [property: DataMember]
        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [property: DataMember]
        [JsonPropertyName("sub_category")]
        public string? SubCategory { get; set; }

        [JsonPropertyName("created_at")]

        [property: DataMember]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [property: DataMember]
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }

}
