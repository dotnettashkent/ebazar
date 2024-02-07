using MemoryPack;
using Shared.Infrastructures;
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
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = null!;

        [property: DataMember]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = null!;

        [property: DataMember]
        [JsonPropertyName("middle_name")]
        public string? MiddleName { get; set; }

        [property: DataMember]
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [property: DataMember]
        [JsonPropertyName("phone_number")]
        public string PhoneNumber { get; set; } = null!;

        [property: DataMember]
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;

        [property: DataMember]
        [JsonPropertyName("gender")]
        public Gender? Gender { get; set; }

        [property: DataMember]
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [property: DataMember]
        [JsonPropertyName("date_of_birth")]
        public DateTime? DateOfBirth { get; set; }

        public virtual CartView? CartView { get; set; }
        public virtual FavouriteView? FavouritesView { get; set; }
        public virtual ICollection<OrderView>? OrdersView { get; set; } = new List<OrderView>();
        public virtual ICollection<AddressView> Addresses { get; set; } = new List<AddressView>();
    }

    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class UserResultView
    {
        [JsonIgnore]
        [property: DataMember] public long Id { get; set; }
        [property: DataMember] public string FirstName { get; set; } = null!;
        [property: DataMember] public string LastName { get; set; } = null!;
        [property: DataMember] public string? MiddleName { get; set; }
        [property: DataMember] public string? Email { get; set; }
        [property: DataMember] public string PhoneNumber { get; set; } = null!;
        [property: DataMember] public string Password { get; set; } = null!;
        [property: DataMember] public Gender? Gender { get; set; }
        [property: DataMember] public DateTime? DateOfBirth { get; set; }
    }
}
