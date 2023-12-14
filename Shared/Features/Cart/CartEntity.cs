using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
	public class CartEntity
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("id")]
		public long Id { get; set; }

		[Column("product_id")]
		public long ProductId { get; set; }

		[Column("user_id")]
		public long UserId { get; set; }

		[Column("product_count")]
		public int ProductCount { get; set; }

		[Column("product_price")]
		public decimal ProductPrice { get; set; }

		[Column("user_money_save")]
		public decimal UserMoneySave { get; set; }
	}
}
