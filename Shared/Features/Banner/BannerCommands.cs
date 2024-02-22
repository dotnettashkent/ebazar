using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    public partial record CreateBannerCommand(
        [property: DataMember] Session Session, 
        [property: DataMember] BannerView Entity,
        [property: DataMember] string? Token) : ISessionCommand<BannerView>;

    [DataContract, MemoryPackable]
    public partial record UpdateBannerCommand(
        [property: DataMember] Session Session, 
        [property: DataMember] BannerView Entity,
        [property: DataMember] string? Token) : ISessionCommand<BannerView>;

    [DataContract, MemoryPackable]
    public partial record DeleteBannerCommand(
        [property: DataMember] Session Session, 
        [property: DataMember] long Id,
        [property: DataMember] string? Token) : ISessionCommand<BannerView>;

}
