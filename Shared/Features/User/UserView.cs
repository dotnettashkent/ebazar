using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class UserView
    {
        [property: DataMember]
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [property: DataMember]
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; } = null!;

        [JsonIgnore]
        [property: DataMember]
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;

        [property: DataMember]
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonIgnore] public virtual CartView? CartView { get; set; }
        [JsonIgnore] public virtual FavouriteView? FavouritesView { get; set; }
        [JsonIgnore] public virtual ICollection<OrderView>? OrdersView { get; set; } = new List<OrderView>();
        [JsonIgnore] public virtual ICollection<AddressView> Addresses { get; set; } = new List<AddressView>();
    }

    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class UserResultView
    {
        [JsonIgnore]
        [property: DataMember] [JsonPropertyName("id")] public long Id { get; set; }
        [property: DataMember] [JsonPropertyName("phone_number")] public string PhoneNumber { get; set; } = null!;
        [property: DataMember] [JsonPropertyName("password")] public string Password { get; set; } = null!;
    }
    public class LoginIncome
    {
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
