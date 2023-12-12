using MemoryPack;
using Stl.Fusion.Blazor;
using Shared.Infrastructures;
using System.Runtime.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class ProductView
    {
        [property : DataMember] public long Id { get; set; }
        [property : DataMember] public string NameEn { get; set; } = null!;
        [property : DataMember] public string NameRu { get; set; } = null!;
        [property : DataMember] public string NameUz { get; set; } = null!;
        [property : DataMember] public string DescriptionEn { get; set; } = null!;
        [property : DataMember] public string DescriptionRu { get; set; } = null!;
        [property : DataMember] public string DescriptionUz { get; set; } = null!;
        [property : DataMember] public int Count { get; set; }
        [property : DataMember] public int MaxCount { get; set; }
        [property : DataMember] public decimal Price { get; set; }
        [property : DataMember] public decimal DiscountPrice { get; set; }
        [property : DataMember] public UnitType UnitType { get; set; }
        [property : DataMember] public bool IsFreeDelivery { get; set; } = false;
        [property : DataMember] public DateTime DeliveryTime { get; set; }
        [property : DataMember] public long PhotoId { get; set; }
        [property : DataMember] public string? Tag {  get; set; }
        [property: DataMember] public bool IsActive { get; set; } = false;
        [property : DataMember] public bool IsPopular { get; set; } = false;
        [property : DataMember] public bool IsHoliday { get; set; } = false;
        [property : DataMember] public bool IsBigSale { get; set; } = false;

        [property: DataMember] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [property: DataMember] public DateTime? UpdatedAt { get; set; }


        public override bool Equals(object? o)
        {
            var other = o as ProductView;
            return other?.Id == Id;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }

}
