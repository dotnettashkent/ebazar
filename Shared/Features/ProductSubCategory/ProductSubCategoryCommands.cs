using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	public partial record CreateProductSubCategoryCommand([property: DataMember] Session Session, [property: DataMember] ProductSubCategoryView Entity) : ISessionCommand<ProductSubCategoryView>;

	[DataContract, MemoryPackable]
	public partial record UpdateProductSubCategoryCommand([property: DataMember] Session Session, [property: DataMember] ProductSubCategoryView Entity) : ISessionCommand<ProductSubCategoryView>;

	[DataContract, MemoryPackable]
	public partial record DeleteProductSubCategoryCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<ProductSubCategoryView>;
}
