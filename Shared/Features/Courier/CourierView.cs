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
		[property : DataMember] public string? LastName { get; set; } 
		[property : DataMember] public string? FirstName { get; set; } 
		[property : DataMember] public string? MiddleName { get; set; } 
		[property : DataMember] public string? PhoneNumber { get; set; } 
		[property : DataMember] public string? Password { get; set; } 
		[property : DataMember] public string? PassportNumber { get; set; } 
		[property : DataMember] public string? PassportLetter { get; set; } 
		[property : DataMember] public string? PassportPINFL { get; set; } 


		[property : DataMember] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		[property : DataMember] public DateTime? UpdatedAt { get; set; }

		//Relations

		public virtual ICollection<OrderEntity> OrderViews { get; set; } = new List<OrderEntity>();
	}
}
