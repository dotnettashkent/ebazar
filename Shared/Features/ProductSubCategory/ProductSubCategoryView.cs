using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class ProductSubCategoryView
	{
		[property: DataMember, JsonPropertyName("id")]
		public long Id { get; set; }
        
		[property : DataMember] [JsonPropertyName("name_uz")]
        public string? NameUz { get; set; }

        [property: DataMember]
		[JsonPropertyName("name_ru")]
		public string? NameRu { get; set; }

		[property : DataMember] [JsonPropertyName("href")]
        public string? Href { get; set; }

		[property: DataMember] [JsonPropertyName("category_id")]
        public long CategoryId { get; set; }

        public override bool Equals(object? o)
		{
			var other = o as ProductCategoryView;
			return other?.Id == Id;
		}
		public override int GetHashCode() => Id.GetHashCode();
	}
}
