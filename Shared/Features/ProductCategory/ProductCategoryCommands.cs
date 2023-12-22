using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	public partial record CreateProductCategoryCommand([property: DataMember] Session Session, [property: DataMember] List<ProductCategoryView >Entity) : ISessionCommand<List<ProductCategoryView>>;

	[DataContract, MemoryPackable]
	public partial record UpdateProductCategoryCommand([property: DataMember] Session Session, [property: DataMember] List<ProductCategoryView >Entity) : ISessionCommand<List<ProductCategoryView>>;

	[DataContract, MemoryPackable]
	public partial record DeleteProductCategoryCommand([property: DataMember] Session Session, [property: DataMember] int Id) : ISessionCommand<List<ProductCategoryView>>;

}
