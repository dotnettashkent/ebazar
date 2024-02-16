using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
    public partial record CreateCourierCommand(
        [property: DataMember] Session Session, 
        [property: DataMember] CourierView Entity,
        [property: DataMember] string Token) : ISessionCommand<CourierView>;

    [DataContract, MemoryPackable]
    public partial record UpdateCourierCommand(
        [property: DataMember] Session Session, 
        [property: DataMember] CourierView Entity,
        [property: DataMember] string Token) : ISessionCommand<CourierView>;

    [DataContract, MemoryPackable]
    public partial record DeleteCourierCommand(
        [property: DataMember] Session Session, 
        [property: DataMember] int Id,
        [property: DataMember] string Token) : ISessionCommand<CourierView>;
}
