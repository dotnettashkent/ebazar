using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    public partial record CreateOrderCommand([property: DataMember] Session Session, [property: DataMember] OrderView Entity) : ISessionCommand<OrderView>;

    [DataContract, MemoryPackable]
    public partial record UpdateOrderCommand(
        [property: DataMember] Session Session,
        [property: DataMember] OrderView Entity) : ISessionCommand<OrderView>;

    [DataContract, MemoryPackable]
    public partial record UpdateItemOrderCommand(
        [property: DataMember] Session Session,
        [property: DataMember] OrderResponse Entity) : ISessionCommand<OrderResponse>;

    [DataContract, MemoryPackable]
    public partial record DeleteOrderCommand([property: DataMember] Session Session, [property: DataMember] OrderView Entity) : ISessionCommand<OrderView>;

}
