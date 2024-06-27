using MemoryPack;
using Stl.Fusion.Blazor;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Shared.Features
{

	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class ProductCategoryView
	{
		[property: DataMember, JsonPropertyName("id")] public long Id { get; set; }

		[property: DataMember]
        [JsonPropertyName("name_uz")]
        public string? NameUz { get; set; } 

        [JsonPropertyName("name_ru")]
        public string? NameRu { get; set; }

        [property: DataMember]
        [JsonPropertyName("slug")]
        public string Slug
        {
            get
            {
                string slug = $"{NameUz}-ProductCategory".ToLower().Replace(" ", "-");
                slug = Regex.Replace(slug, @"[^a-z0-9\-]+", "");
                slug = Regex.Replace(slug, @"\-{2,}", "-");
                slug = slug.Trim('-');
                return slug;
            }
        }

        public override bool Equals(object? o)
		{
			var other = o as ProductCategoryView;
			return other?.Id == Id;
		}
		public override int GetHashCode() => Id.GetHashCode();
	}
}
