﻿using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class ProductView
    {
        [property : DataMember] public long Id { get; set; }
        [property : DataMember] public string Locale { get; set; } = null!;
        [property : DataMember] public string? Name { get; set; } 
        [property : DataMember] public string? Description { get; set; } 
        [property : DataMember] public string? BrandName { get; set; } 
        [property : DataMember] public int? Count { get; set; }
        [property : DataMember] public int? MaxCount { get; set; }
        [property : DataMember] public decimal? Price { get; set; }
        [property : DataMember] public decimal? DiscountPrice { get; set; }
        [property : DataMember] public string? PriceType { get; set; }
        [property : DataMember] public bool IsFreeDelivery { get; set; } = false;
        [property : DataMember] public DateTime DeliveryTime { get; set; }
        [property : DataMember] public string? Photo { get; set; }
        [property : DataMember] public string? Tag {  get; set; }
        [property : DataMember] public decimal? Weight { get; set; }
        [property : DataMember] public bool IsActive { get; set; } = false;
        [property : DataMember] public bool IsPopular { get; set; } = false;
        [property : DataMember] public bool IsHoliday { get; set; } = false;
        [property : DataMember] public bool IsBigSale { get; set; } = false;

		[property: DataMember] public string? Category { get; set; }
		[property : DataMember] public string? SubCategory { get; set; }

		[property : DataMember] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [property : DataMember] public DateTime? UpdatedAt { get; set; }

		//Relations
		[JsonIgnore] public virtual CartView? CartView { get; set; }
		[JsonIgnore] public virtual ICollection<OrderView> OrdersView { get; set; } = new List<OrderView>();
		[JsonIgnore] public virtual FavouriteView? FavouriteView { get; set; }


		public override bool Equals(object? o)
        {
            var other = o as ProductView;
            return other?.Id == Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }

}
