using EF.Audit;
using MemoryPack;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
	public class OrderEntity
	{
		[Column("id")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[Column("cart_id")]
		public long CartId { get; set; }

		[Column("user_id")]
		public long UserId { get; set; }

		[Column("courier_id")]
		public long CourierId { get; set; }

		[Column("is_success")]
		public bool IsSuccess { get; set; } = false;

		[Column("user_comment")]
		public string UserComment { get; set; } = string.Empty;

		//Relations
		public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
		public virtual UserEntity? User {  get; set; }
		public virtual CourierEntity? Courier { get; set; }
	}
}
