using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
	[Table("product_category")]
	public class ProductCategoryEntity
	{
		[Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
		public long Id { get; set; }

		[Column("name")]
		public string Name { get; set; } = null!;
	}
}
