﻿/*using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared.Features
{

	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class ProductCategoryView
	{
		[property : DataMember] public long Id { get; set; }
		[property: DataMember] public string Name { get; set; } = null!;

		public override bool Equals(object? o)
		{
			var other = o as ProductCategoryView;
			return other?.Id == Id;
		}
		public override int GetHashCode() => Id.GetHashCode();
	}
}
*/