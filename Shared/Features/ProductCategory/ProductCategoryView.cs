﻿using MemoryPack;
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
		[property : DataMember] public string MainNameUz { get; set; }
		[property : DataMember] public string MainNameRu { get; set; }
		[property : DataMember] public string MainNameEn { get; set; }
		[property : DataMember] public string MainLinkUz { get; set; }
		[property : DataMember] public string MainLinkRu { get; set; }
		[property : DataMember] public string MainLinkEn { get; set; }
		[property : DataMember] public string SecondNameUz { get; set; }
		[property : DataMember] public string SecondNameRu { get; set; }
		[property : DataMember] public string SecondNameEn { get; set; }
		[property : DataMember] public string SecondLinkUz { get; set; }
		[property : DataMember] public string SecondLinkRu { get; set; }
		[property : DataMember] public string SecondLinkEn { get; set; }
		[property : DataMember] public string ThirdNameUz { get; set; }
		[property : DataMember] public string ThirdNameRu { get; set; }
		[property : DataMember] public string ThirdNameEn { get; set; }
		[property : DataMember] public string ThirdLinkUz { get; set; }
		[property : DataMember] public string ThirdLinkRu { get; set; }
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
