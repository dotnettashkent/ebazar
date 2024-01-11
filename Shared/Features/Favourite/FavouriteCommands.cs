using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	public partial record CreateFavouriteCommand(
		[property: DataMember] Session Session,
		[property: DataMember] FavouriteView Entity) : ISessionCommand<FavouriteView>;


	[DataContract, MemoryPackable]
	public partial record DeleteFavouriteCommand(
		[property: DataMember] Session Session,
        [property: DataMember] FavouriteView Entity) : ISessionCommand<FavouriteView>;

}
