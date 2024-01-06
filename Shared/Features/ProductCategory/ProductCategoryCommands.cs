using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	public partial record CreateProductCategoryCommand([property: DataMember] Session Session, [property: DataMember] ProductCategoryView Entity) : ISessionCommand<ProductCategoryView>;

	[DataContract, MemoryPackable]
	public partial record UpdateProductCategoryCommand([property: DataMember] Session Session, [property: DataMember] ProductCategoryView Entity) : ISessionCommand<ProductCategoryView>;

	[DataContract, MemoryPackable]
	public partial record DeleteProductCategoryCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<ProductCategoryView>;

}
