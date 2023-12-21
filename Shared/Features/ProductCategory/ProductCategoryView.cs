using MemoryPack;
using Shared.Features.File;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared.Features
{

	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class ProductCategoryView
	{
		[property : DataMember] public long Id { get; set; }
		[property : DataMember] public string? Locale { get; set; }
		[property : DataMember] public string? MainName { get; set; }
		[property : DataMember] public string? MainLink { get; set; }
		[property : DataMember] public string? SecondName { get; set; }
		[property : DataMember] public string? SecondLink { get; set; }
		[property : DataMember] public string? ThirdName { get; set; }
		[property : DataMember] public string? ThirdLink { get; set; }
		[property : DataMember] public bool IsPopular { get; set; }
		[property : DataMember] public FileEntity? PhotoView { get; set; }
		[property : DataMember] public FileEntity? PhotoMobileView { get; set; }

		public override bool Equals(object? o)
		{
			var other = o as ProductCategoryView;
			return other?.Id == Id;
		}
		public override int GetHashCode() => Id.GetHashCode();
	}
}
