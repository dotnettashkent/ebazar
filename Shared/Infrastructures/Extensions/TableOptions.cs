using MemoryPack;
using System.Runtime.Serialization;

namespace Shared.Infrastructures
{
    [DataContract, MemoryPackable]
    public sealed partial record TableOptions
    {
    }
}
