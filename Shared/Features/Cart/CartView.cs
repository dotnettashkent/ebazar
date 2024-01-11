using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class CartView
	{
		[property : DataMember] public long Id { get; set; }
		[property: DataMember] public List<long> Products { get; set; } = new List<long>();
		[property : DataMember] public long UserId { get; set; }
		
		
		[property : DataMember] public virtual ICollection<ProductView> ProductsView { get; set; } = new List<ProductView>();
		[property : DataMember] public virtual UserView? UserView { get; set; }
	}
}
