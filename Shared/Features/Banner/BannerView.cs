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
    public partial class BannerView
    {
        [property : DataMember]
        [property : JsonPropertyName("id")]
        public long Id { get; set; }

        [property: DataMember]
        [property: JsonPropertyName("link")]
        public string? Link { get; set; }
        
        [property: DataMember]
        [property: JsonPropertyName("image")]
        public string? Photo { get; set; }

        [property: DataMember]
        [property: JsonPropertyName("sort")]

        public int Sort { get; set; }



        [NotMapped]
        [property: DataMember]
        [JsonPropertyName("photo")]
        [MemoryPackAllowSerialize]
        [JsonIgnore]
        public IFormFile? PhotoView { get; set; }
    }
}
