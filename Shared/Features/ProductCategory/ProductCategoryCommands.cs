using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	public partial record CreateProductCategoryCommand(
		[property: DataMember] Session Session, 
		[property: DataMember] ProductCategoryView Entity,
		[property: DataMember] string? Token) : ISessionCommand<ProductCategoryView>;

	[DataContract, MemoryPackable]
	public partial record UpdateProductCategoryCommand(
		[property: DataMember] Session Session, 
		[property: DataMember] ProductCategoryView Entity,
        [property: DataMember] string? Token) : ISessionCommand<ProductCategoryView>;

	[DataContract, MemoryPackable]
	public partial record DeleteProductCategoryCommand(
		[property: DataMember] Session Session, 
		[property: DataMember] long Id,
        [property: DataMember] string? Token) : ISessionCommand<ProductCategoryView>;

}
