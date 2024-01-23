using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	public partial record CreateCartCommand(
		[property : DataMember] Session Session,
		[property : DataMember] CartView Entity) : ISessionCommand<CartView>;


	[DataContract, MemoryPackable]
	public partial record DeleteCartCommand(
		[property : DataMember] Session Session, 
		[property : DataMember] CartView Entity) : ISessionCommand<CartView>;

}
