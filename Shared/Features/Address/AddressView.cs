using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class AddressView
    {
        [property: DataMember][JsonPropertyName("id")] public long Id { get; set; }
        [property: DataMember][JsonPropertyName("user_id")] public long UserId { get; set; }
        [property: DataMember][JsonPropertyName("region")] public string? Region { get; set; }
        [property: DataMember][JsonPropertyName("district")] public string? District { get; set; }
        [property: DataMember][JsonPropertyName("street")] public string? Street { get; set; }
        [property: DataMember][JsonPropertyName("home_number")] public string? HomeNumber { get; set; }
        [property: DataMember][JsonPropertyName("home_or_office")] public int HomeOrOffice { get; set; }
        [property: DataMember][JsonPropertyName("domophone_code")] public string? DomophoneCode { get; set; }

    }
}
