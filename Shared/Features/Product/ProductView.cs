using MemoryPack;
using Microsoft.AspNetCore.Http;
using Stl.Fusion.Blazor;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class ProductView
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
        [JsonPropertyName("info_count")]
        public int? InfoCount { get; set; }

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
        public bool IsFreeDelivery { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_1")]
        public string? PhotoOne { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_2")]
        public string? PhotoTwo { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_3")]
        public string? PhotoThree { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_4")]
        public string? PhotoFour { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_5")]
        public string? PhotoFive { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_6")]
        public string? PhotoSix { get; set; }


        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageOne { get; set; }

        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageTwo { get; set; }

        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageThree { get; set; }

        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageFour { get; set; }

        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageFive { get; set; }

        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageSix { get; set; }


        [property: DataMember]
        [JsonPropertyName("tag")]
        [JsonIgnore]
        public string? Tag { get; set; }

        [property: DataMember]
        [JsonPropertyName("weight")]
        public decimal? Weight { get; set; }

        [property: DataMember]
        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [property: DataMember]
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }


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
        [JsonPropertyName("info_count")]
        public int? InfoCount { get; set; }

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
        public bool IsDeliveryFree { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_1")]
        public string? PhotoOne { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_2")]
        public string? PhotoTwo { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_3")]
        public string? PhotoThree { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_4")]
        public string? PhotoFour { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_5")]
        public string? PhotoFive { get; set; }

        [property: DataMember]
        [JsonPropertyName("photo_6")]
        public string? PhotoSix { get; set; }


        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageOne { get; set; }

        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageTwo { get; set; }

        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageThree { get; set; }

        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageFour { get; set; }

        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageFive { get; set; }

        [NotMapped]
        [JsonIgnore]
        [property: DataMember]
        [MemoryPackAllowSerialize]
        public IFormFile? ImageSix { get; set; }

        [property: DataMember]
        [JsonPropertyName("tag")]
        [JsonIgnore]
        public string? Tag { get; set; }

        [property: DataMember]
        [JsonPropertyName("weight")]
        public decimal? Weight { get; set; }

        [property: DataMember]
        [JsonPropertyName("unit")]
        public string? Unit { get; set; }

        [property: DataMember]
        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }




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


        [property: DataMember]
        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

    }

}
