using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class OrderView
	{
        [property: DataMember][JsonPropertyName("id")] public long Id { get; set; }
        [property: DataMember][JsonPropertyName("product_id")] public List<long> ProductIds { get; set; } = new List<long>();
        [property: DataMember][JsonPropertyName("user_id")] public long UserId { get; set; }

        [JsonIgnore][property: DataMember] public virtual ICollection<ProductView> ProductsView { get; set; } = new List<ProductView>();
        [JsonIgnore][property: DataMember] public virtual UserView? UserView { get; set; }
    }
}
