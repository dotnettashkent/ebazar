using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class BrandView
	{
		[property: DataMember] public long Id { get; set; }
		[property: DataMember] public string Name { get; set; } = null!;
		[property: DataMember] public string IsPopular { get; set; } = null!;
		[property: DataMember] public long PhotoId { get; set; }

	}
}
