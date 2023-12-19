
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features.Favourite
{
	public class FavouriteEntity
	{
		[Column("id")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[Column("user_id")]
		public long UserId { get; set; }

		[Column("product_id")]
		public long ProductId { get; set; }
	}
}
