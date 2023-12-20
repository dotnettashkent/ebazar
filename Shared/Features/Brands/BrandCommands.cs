using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	public partial record CreateBrandCommand([property: DataMember] Session Session, [property: DataMember] List<BrandView> Entity) : ISessionCommand<List<BrandView>>;

	[DataContract, MemoryPackable]
	public partial record UpdateBrandCommand([property: DataMember] Session Session, [property: DataMember] List<BrandView> Entity) : ISessionCommand<List<BrandView>>;

	[DataContract, MemoryPackable]
	public partial record DeleteBrandCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<List<BrandView>>;
}
