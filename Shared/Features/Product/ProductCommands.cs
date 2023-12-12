using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    public partial record CreateProductCommand([property: DataMember] Session Session, [property: DataMember] ProductView Entity) : ISessionCommand<ProductView>;

    [DataContract, MemoryPackable]
    public partial record UpdateProductCommand([property: DataMember] Session Session, [property: DataMember] ProductView Entity) : ISessionCommand<ProductView>;

    [DataContract, MemoryPackable]
    public partial record DeleteProductCommand([property: DataMember] Session Session, [property: DataMember] int Id) : ISessionCommand<ProductView>;

}
