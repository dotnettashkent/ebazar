using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
namespace Shared.Features
{

    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class FavouriteView
    {
        [property: DataMember][JsonPropertyName("id")] public long Id { get; set; }
        [property: DataMember][JsonPropertyName("user_id")] public long UserId { get; set; }
        [property: DataMember][JsonPropertyName("products")] public List<long> Products { get; set; } = new List<long>();

        //Relations
        [JsonIgnore] public virtual ICollection<ProductView>? ProductView { get; set; }
        [JsonIgnore] public virtual UserView? UserView { get; set; }
    }
}
