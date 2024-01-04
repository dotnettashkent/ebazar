using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
	[Table("carts")]
	public class CartEntity
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("id")]
		public long Id { get; set; }

		[Column("product_ids")]
		public List<string>? ProductIds { get; set; }

		[Column("user_id")]
		public long UserId { get; set; }


		public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
		public virtual UserEntity? User { get; set; }


	}
}
