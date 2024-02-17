using MemoryPack;
using Shared.Infrastructures;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Features
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class OrderView
    {
        [property: DataMember]
        [JsonPropertyName("id")]
        public long Id { get; set; }


        [property: DataMember]
        [JsonPropertyName("token")]
        public string Token { get; set; } = null!;


        [property: DataMember]
        [JsonPropertyName("city")]
        public string? City { get; set; }


        [property: DataMember]
        [JsonPropertyName("region")]
        public string? Region { get; set; }



        [property: DataMember]
        [JsonPropertyName("street")]
        public string? Street { get; set; }


        [property: DataMember]
        [JsonPropertyName("home_number")]
        public string? HomeNumber { get; set; }


        [property: DataMember]
        [JsonPropertyName("comment_for_courier")]
        public string? CommentForCourier { get; set; }


        [property: DataMember]
        [JsonPropertyName("delivery_time")]
        public string? DeliveryTime { get; set; }


        [property: DataMember]
        [JsonPropertyName("payment_type")]
        public string? PaymentType { get; set; }


        [property: DataMember]
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }


        [property: DataMember]
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }


        [property: DataMember]
        [JsonPropertyName("extra_phone_number")]
        public string? ExtraPhoneNumber { get; set; }

        [property: DataMember]
        public string? Status { get; set; }

        [JsonIgnore]
        [property: DataMember]
        [JsonPropertyName("products")]

        public List<ProductResultView> Product { get; set; } = new();

    }
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class OrderResponse
    {
        [property: DataMember]
        [JsonPropertyName("user_id")]
        public long UserId { get; set; }


        [property: DataMember]
        [JsonPropertyName("city")]
        public string? City { get; set; }


        [property: DataMember]
        [JsonPropertyName("region")]
        public string? Region { get; set; }



        [property: DataMember]
        [JsonPropertyName("street")]
        public string? Street { get; set; }


        [property: DataMember]
        [JsonPropertyName("home_number")]
        public string? HomeNumber { get; set; }


        [property: DataMember]
        [JsonPropertyName("comment_for_courier")]
        public string? CommentForCourier { get; set; }


        [property: DataMember]
        [JsonPropertyName("delivery_time")]
        public string? DeliveryTime { get; set; }


        [property: DataMember]
        [JsonPropertyName("payment_type")]
        public string? PaymentType { get; set; }


        [property: DataMember]
        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }


        [property: DataMember]
        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }


        [property: DataMember]
        [JsonPropertyName("extra_phone_number")]
        public string? ExtraPhoneNumber { get; set; }


        [property: DataMember]
        public string Status { get; set; } = OrderStatus.Pending.ToString();

        [property: DataMember]
        [JsonPropertyName("products")]
        public List<ProductResultView> Product { get; set; } = new();
    }
}
