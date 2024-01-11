using MemoryPack;
using Shared.Features;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared;

[DataContract, MemoryPackable]
[ParameterComparer(typeof(ByValueParameterComparer))]
public partial class LocaleView
{
   [property: DataMember] public string Code { get; set; } = null!;
   [property: DataMember] public long Id { get; set; }
   [property: DataMember] public ICollection<ProductView> ProductView { get; set; } = new List<ProductView>();

    public override bool Equals(object? o)
    {
        var other = o as LocaleView;
        return other?.Code == Code;
    }
    public override int GetHashCode() => Code.GetHashCode();
}
