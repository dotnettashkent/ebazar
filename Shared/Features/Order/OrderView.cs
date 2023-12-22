using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class OrderView
	{
		[property : DataMember] public long Id { get; set; }
		[property : DataMember] public long CartId { get; set; }
		[property : DataMember] public long UserId { get; set; }
		[property : DataMember] public long CourierId { get; set; }
		[property : DataMember] public bool IsSuccess { get; set; } = false;
		[property : DataMember] public string UserComment { get; set; } = string.Empty;

		//Relations
		public virtual ICollection<ProductView> Products { get; set; } = new List<ProductView>();
		public virtual UserView? User { get; set; }
		public virtual CourierView? Courier { get; set; }
	}
}
