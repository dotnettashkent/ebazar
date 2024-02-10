using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    public partial record CreateUserCommand(
        [property: DataMember] Session Session, 
        [property: DataMember] UserResultView Entity) : ISessionCommand<bool>;

    [DataContract, MemoryPackable]
    public partial record UpdateUserCommand(
        [property : DataMember] Session Session, 
        [property : DataMember] UserResultView Entity,
        [property : DataMember] long UserId) : ISessionCommand<UserView>;

    [DataContract, MemoryPackable]
    public partial record DeleteUserCommand([property: DataMember] Session Session, [property: DataMember] int Id) : ISessionCommand<UserView>;
}
