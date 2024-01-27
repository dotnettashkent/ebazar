using MemoryPack;
using Microsoft.AspNetCore.Http;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class BrandView
	{
		[property : DataMember] [JsonPropertyName("id")] public long Id { get; set; }
		[property : DataMember] [JsonPropertyName("name")] public string? Name { get; set; }
		[property : DataMember] [JsonPropertyName("is_popular")] public bool IsPopular { get; set; } = false;
		[property : DataMember] [JsonPropertyName("link")] public string? Link { get; set; }
		[property : DataMember] [JsonPropertyName("photo")] public IFormFile? PhotoView { get; set; }

	}
}
