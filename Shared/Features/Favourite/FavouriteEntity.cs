
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
	[Table("favourites")]
	public class FavouriteEntity
	{
		[Column("id")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[Column("user_id")]
		public long UserId { get; set; }

		[Column("product_id")]
		public List<long> ProductIds { get; set; } = new List<long>();

		//Relations
		public virtual ICollection<ProductEntity> Products { get; set; } = new List<ProductEntity>();
		public virtual UserEntity? User { get; set; }
	}
}
