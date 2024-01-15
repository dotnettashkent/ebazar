using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class BannerView
    {
        [property : DataMember] [JsonPropertyName("id")] public long Id { get; set; }
        [property : DataMember] [JsonPropertyName("locale")] public string Locale { get; set; } = null!;
        [property : DataMember] [JsonPropertyName("photo")] public string PhotoView { get; set; } = null!;
        [property : DataMember] [JsonPropertyName("title")] public string Title { get; set; } = null!;
        [property : DataMember] [JsonPropertyName("link")] public string Link { get; set; } = null!;
        [property : DataMember] [JsonPropertyName("description")] public string? Description { get; set; }
        [property : DataMember] [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [property : DataMember] [JsonPropertyName("updated_at")] public DateTime? UpdatedAt { get; set; }
    }
}
