using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class CartView
	{
		public long Id { get; set; }
		public long ProductId { get; set; }
		public long UserId { get; set; }
		public int ProductCount { get; set; }
		public decimal ProductPrice { get; set; }
		//public decimal UserMoneySave { get; set; }
		public virtual ICollection<ProductView> Products { get; set; } = new List<ProductView>();
		public virtual UserView? User { get; set; }
	}
}
