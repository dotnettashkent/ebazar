using MemoryPack;
using Shared.Infrastructures;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class UserView
	{
		[property : DataMember] public long Id { get; set; }
		[property : DataMember] public string FirstName { get; set; } = null!;
		[property : DataMember] public string LastName { get; set; } = null!;
		[property : DataMember] public string? MiddleName { get; set; }
		[property : DataMember] public string? Email { get; set; }
		[property : DataMember] public string PhoneNumber { get; set; } = null!;
		[property : DataMember] public string Password { get; set; } = null!;
		[property : DataMember] public Gender Gender { get; set; }
		[property : DataMember] public DateTime? DateOfBirth { get; set; }

	}
}
