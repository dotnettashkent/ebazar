using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
	[DataContract, MemoryPackable]
	[ParameterComparer(typeof(ByValueParameterComparer))]
	public partial class OrderView
	{
		[property : DataMember] [JsonPropertyName("id")] public long Id { get; set; }
		[property : DataMember] [JsonPropertyName("cart_id")] public long CartId { get; set; }
		[property : DataMember] [JsonPropertyName("user_id")] public long UserId { get; set; }
		[property : DataMember] [JsonPropertyName("courier_id")] public long CourierId { get; set; }
		[property : DataMember] [JsonPropertyName("is_success")] public bool IsSuccess { get; set; } = false;
		[property : DataMember] [JsonPropertyName("user_commment")] public string UserComment { get; set; } = string.Empty;

		//Relations
		public virtual ICollection<ProductView> ProductsView { get; set; } = new List<ProductView>();
		public virtual UserView? UserView { get; set; }
		public virtual CourierView? CourierView { get; set; }
	}
}
