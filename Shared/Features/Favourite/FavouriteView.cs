using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
namespace Shared.Features
{

	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class FavouriteView
	{
		[property : DataMember]	public long Id { get; set; }
		[property : DataMember]	public long UserId { get; set; }
		[property : DataMember] public List<long> ProductIds { get; set; } = new List<long>();

		//Relations
		public virtual ICollection<ProductView> ProductsView { get; set; } = new List<ProductView>();
		public virtual UserView? UserView { get; set; }
	}
}
