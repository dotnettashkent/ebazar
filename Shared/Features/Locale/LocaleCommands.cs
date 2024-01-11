using MemoryPack;
using Stl.CommandR;
using Stl.Fusion;
using System.Runtime.Serialization;


namespace Shared;


[DataContract, MemoryPackable]
public partial record CreateLocaleCommand([property: DataMember] Session Session,[property: DataMember] LocaleView Entity):ISessionCommand<LocaleView>; 

[DataContract, MemoryPackable]
public partial record UpdateLocaleCommand([property: DataMember] Session Session,[property: DataMember] LocaleView Entity):ISessionCommand<LocaleView>; 

[DataContract, MemoryPackable]
public partial record DeleteLocaleCommand([property: DataMember] Session Session,[property: DataMember] string Code):ISessionCommand<LocaleView>; 

