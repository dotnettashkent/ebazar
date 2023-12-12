using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class BannerView
    {
        [property : DataMember] public long Id { get; set; }
        [property : DataMember] public FileView Photo { get; set; } = null!;
        [property : DataMember] public string Title { get; set; } = null!;
        [property : DataMember] public string? Description { get; set; }

        [property : DataMember] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [property: DataMember] public DateTime? UpdatedAt { get; set; }
    }
}
