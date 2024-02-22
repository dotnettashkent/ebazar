using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	public partial record CreateProductSubCategoryCommand(
		[property: DataMember] Session Session, 
		[property: DataMember] ProductSubCategoryView Entity, 
		[property: DataMember] string? Token = "") : ISessionCommand<ProductSubCategoryView>;

	[DataContract, MemoryPackable]
	public partial record UpdateProductSubCategoryCommand(
		[property: DataMember] Session Session, 
		[property: DataMember] ProductSubCategoryView Entity, 
		[property: DataMember] string? Token = "") : ISessionCommand<ProductSubCategoryView>;

	[DataContract, MemoryPackable]
	public partial record DeleteProductSubCategoryCommand(
		[property: DataMember] Session Session, 
		[property: DataMember] long Id,
		[property: DataMember] string? Token = "") : ISessionCommand<ProductSubCategoryView>;
}
