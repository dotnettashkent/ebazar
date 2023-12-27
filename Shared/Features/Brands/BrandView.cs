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
		[property: DataMember] public string? Name { get; set; }
		[property: DataMember] public bool IsPopular { get; set; } = false;
		[property : DataMember] public string? Link { get; set; }
		//[property: DataMember] public FileView? PhotoView { get; set; }

	}
}
