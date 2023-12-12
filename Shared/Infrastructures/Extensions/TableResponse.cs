using MemoryPack;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Shared.Infrastructures.Extensions
{
    [DataContract, MemoryPackable]
    public partial class TableResponse<T> where T : class
    {
        [property: DataMember][JsonProperty("items")] public List<T> Items { get; set; } = new List<T>();

        [property: DataMember][JsonProperty("total_items")] public int TotalItems { get; set; }
    }
}
