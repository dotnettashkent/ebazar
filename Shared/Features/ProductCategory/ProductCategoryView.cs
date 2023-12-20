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
		[property : DataMember] public string MainNameUz { get; set; } = null!;
		[property : DataMember] public string MainNameRu { get; set; } = null!;
		[property : DataMember] public string MainNameEn { get; set; } = null!;
		[property : DataMember] public string MainLinkUz { get; set; } = null!;
		[property : DataMember] public string MainLinkRu { get; set; } = null!;
		[property : DataMember] public string MainLinkEn { get; set; } = null!;
		[property : DataMember] public string SecondNameUz { get; set; } = null!;
		[property : DataMember] public string SecondNameRu { get; set; } = null!;
		[property : DataMember] public string SecondNameEn { get; set; } = null!;
		[property : DataMember] public string SecondLinkUz { get; set; } = null!;
		[property : DataMember] public string SecondLinkRu { get; set; } = null!;
		[property : DataMember] public string SecondLinkEn { get; set; } = null!;
		[property : DataMember] public string ThirdNameUz { get; set; } = null!;
		[property : DataMember] public string ThirdNameRu { get; set; } = null!;
		[property : DataMember] public string ThirdNameEn { get; set; } = null!;
		[property : DataMember] public string ThirdLinkUz { get; set; } = null!;
		[property : DataMember] public string ThirdLinkRu { get; set; } = null!;
		[property : DataMember] public bool IsPopular { get; set; }
		[MemoryPackAllowSerialize]
		[property : DataMember] public FileEntity? PhotoView { get; set; }
		[MemoryPackAllowSerialize]
		[property : DataMember] public FileEntity? PhotoMobileView { get; set; }

		public override bool Equals(object? o)
		{
			var other = o as ProductCategoryView;
			return other?.Id == Id;
		}
		public override int GetHashCode() => Id.GetHashCode();
	}
}
