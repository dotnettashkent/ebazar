﻿using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class CartView
	{
		[property : DataMember] public long Id { get; set; }
		[property: DataMember] public List<long> ProductIds { get; set; } = new List<long>();
		[property : DataMember] public long UserId { get; set; }
		
		
		[JsonIgnore] [property : DataMember] public virtual ICollection<ProductView> ProductsView { get; set; } = new List<ProductView>();
		[JsonIgnore] [property : DataMember] public virtual UserView? UserView { get; set; }
	}
}
