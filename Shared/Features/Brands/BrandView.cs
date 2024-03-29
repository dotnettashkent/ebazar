using MemoryPack;
using Microsoft.AspNetCore.Http;
using Stl.Fusion.Blazor;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class BrandView
    {
        [property: DataMember][JsonPropertyName("id")] public long Id { get; set; }
        [property: DataMember][JsonPropertyName("name")] public string? Name { get; set; }
        [property: DataMember][JsonPropertyName("is_popular")] public bool IsPopular { get; set; }
        [property: DataMember][JsonPropertyName("link")] public string? Link { get; set; }

        [NotMapped]
        [property: DataMember]
        [JsonPropertyName("photo")]
        [MemoryPackAllowSerialize]
        [JsonIgnore]
        public IFormFile? Photo { get; set; }

        private string? _imageOne;

        [property: DataMember]
        [JsonPropertyName("brand_image")]
        public string? ImageOne
        {
            get => _imageOne;
            set
            {
                if (value != null)
                {
                    // Remove "www" from the URL
                    _imageOne = value.Replace("www.", "");
                }
                else
                {
                    _imageOne = null;
                }
            }
        }
    }
}
