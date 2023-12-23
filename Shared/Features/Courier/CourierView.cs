using MemoryPack;
using Stl.Fusion.Blazor;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Shared.Features
{

	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class CourierView
	{
		[property : DataMember] public long Id { get; set; }
		[property : DataMember] public long OrderId { get; set; }
		[property : DataMember] public string LastName { get; set; } = null!;
		[property : DataMember] public string FirstName { get; set; } = null!;
		[property : DataMember] public string MiddleName { get; set; } = null!;
		[property : DataMember] public string PhoneNumber { get; set; } = null!;
		[property : DataMember] public string Password { get; set; } = null!;
		[property : DataMember] public string PassportNumber { get; set; } = null!;
		[property : DataMember] public string PassportLetter { get; set; } = null!;
		[property : DataMember] public string PassportPINFL { get; set; } = null!;
	}
}
