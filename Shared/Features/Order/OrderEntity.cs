using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
	[Table("orders")]
	public class OrderEntity
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        

        [Column("city")]
        public string City { get; set; } = null!;

        [Column("region")]
        public string Region { get; set; } = null!;

        [Column("street")]
        public string Street { get; set; } = null!;

        [Column("home_number")]
        public string HomeNumber { get; set; } = null!;

        [Column("comment_for_courier")]
        public string CommentForCourier { get; set; } = null!;

        [Column("delivery_time")]
        public string? DeliveryTime { get; set; } = null!;

        [Column("payment_type")]
        public string PaymentType { get; set; } = null!;

        [Column("first_name")]
        public string FirstName { get; set; } = null!;

        [Column("last_name")]
        public string LastName { get; set; } = null!;

        [Column("extra_phone_number")]
        public string ExtraPhoneNumber { get; set; } = null!;

        [Column("status")]
        public string? Status { get; set; }


        [Column("products")]
        public string? Products {  get; set; }

    }
}
