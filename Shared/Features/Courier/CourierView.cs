using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{

	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class CourierView
	{
		[property : DataMember] [JsonPropertyName("id")] public long Id { get; set; }
		[property : DataMember] [JsonPropertyName("order_id")] public long OrderId { get; set; }
		[property : DataMember] [JsonPropertyName("last_name")] public string? LastName { get; set; } 
		[property : DataMember] [JsonPropertyName("first_name")] public string? FirstName { get; set; } 
		[property : DataMember] [JsonPropertyName("middle_name")] public string? MiddleName { get; set; } 
		[property : DataMember] [JsonPropertyName("phone_number")] public string? PhoneNumber { get; set; } 
		[property : DataMember] [JsonPropertyName("password")] public string? Password { get; set; } 
		[property : DataMember] [JsonPropertyName("passport_number")] public string? PassportNumber { get; set; } 
		[property : DataMember] [JsonPropertyName("passport_letter")] public string? PassportLetter { get; set; } 
		[property : DataMember] [JsonPropertyName("passport_PINFL")] public string? PassportPINFL { get; set; } 

		[property : DataMember] [JsonPropertyName("created_at")] public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		[property : DataMember] [JsonPropertyName("updated_at")] public DateTime? UpdatedAt { get; set; }

		//Relations

		public virtual ICollection<OrderEntity> OrderViews { get; set; } = new List<OrderEntity>();
	}
}
