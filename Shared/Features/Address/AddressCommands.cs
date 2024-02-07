using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    public partial record CreateAddressCommand([property: DataMember] Session Session, [property: DataMember] AddressView Entity) : ISessionCommand<AddressView>;

    [DataContract, MemoryPackable]
    public partial record UpdateAddressCommand([property: DataMember] Session Session, [property: DataMember] AddressView Entity) : ISessionCommand<AddressView>;

    [DataContract, MemoryPackable]
    public partial record DeleteAddressCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<AddressView>;
}
