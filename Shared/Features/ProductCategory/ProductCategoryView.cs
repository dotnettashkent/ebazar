using MemoryPack;
using Stl.Fusion.Blazor;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{

	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class ProductCategoryView
	{
		[property: DataMember, JsonPropertyName("id")] public long Id { get; set; }

		[property: DataMember]
        [JsonPropertyName("name_uz")]
        public string NameUz { get; set; } = null!;

        [JsonPropertyName("name_ru")]
        public string NameRu { get; set; } = null!;

        public override bool Equals(object? o)
		{
			var other = o as ProductCategoryView;
			return other?.Id == Id;
		}
		public override int GetHashCode() => Id.GetHashCode();
	}
}
