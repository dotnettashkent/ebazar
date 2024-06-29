using MemoryPack;
using Stl.Fusion.Blazor;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class CartView
    {
        [property: DataMember]
        [JsonPropertyName("id")]
        public long Id { get; set; }


        [property: DataMember]
        [JsonPropertyName("products")]
        public List<ProductList> Products { get; set; } = new();


        [property: DataMember]
        [JsonPropertyName("token")]
        [JsonIgnore]
        [NotMapped]
        public string? Token { get; set; }

        [property: DataMember]
        [property: JsonIgnore]
        public long UserId { get; set; }

        [JsonIgnore]
        [property: DataMember]
        public virtual ICollection<ProductView> ProductsView { get; set; } = new List<ProductView>();

        [JsonIgnore]
        [property: DataMember]
        public virtual UserView? UserView { get; set; }
    }

    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class ProductList
    {
        [property: DataMember]
        [JsonPropertyName("product_id")]
        public long ProductId { get; set; }


        [property: DataMember]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

    }
}
