﻿using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    public partial record CreateProductCommand([property: DataMember] Session Session, [property: DataMember] ProductResultView Entity, string Token) : ISessionCommand<ProductResultView>;

    [DataContract, MemoryPackable]
    public partial record UpdateProductCommand([property: DataMember] Session Session, [property: DataMember] ProductResultView Entity) : ISessionCommand<ProductResultView>;

    [DataContract, MemoryPackable]
    public partial record DeleteProductCommand([property: DataMember] Session Session, [property: DataMember] int Id) : ISessionCommand<ProductResultView>;

}
