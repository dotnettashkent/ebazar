using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class AddressView
	{
		[property : DataMember] public long Id { get; set; }
		[property : DataMember] public long UserId { get; set; }
		[property : DataMember] public string? Region { get; set; } 
		[property : DataMember] public string? District { get; set; } 
		[property : DataMember] public string? Street { get; set; } 
		[property : DataMember] public string? HomeNumber { get; set; } 
		[property : DataMember] public int HomeOrOffice { get; set; }
		[property : DataMember] public string? DomophoneCode { get; set; }
		[property : DataMember] public string? DeliveryComment { get; set; }

	}
}
