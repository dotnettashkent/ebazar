using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared.Features.ProductDiscount
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class ProductDiscountVeiw
	{
		[property : DataMember] public long Id { get; set; }
		[property : DataMember] public long? ProductId { get; set; }
		[property : DataMember] public int? Count { get; set; }
		[property : DataMember] public short? Percent { get; set; }
		[property : DataMember] public decimal? Price { get; set; }
	}
}
