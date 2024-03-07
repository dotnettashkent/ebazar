using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
	[Table("product_sub_category")]
	public class ProductSubCategoryEntity
	{
		[Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
		public long Id { get; set; }

        [Column("name_uz")]
        public string NameUz { get; set; } = null!;

        [Column("name_ru")]
        public string NameRu { get; set; } = null!;

        [Column("href")]
        public string Href { get; set; } = null!;

        [Column("category_id")]
        public long CategoryId { get; set; }
    }
}
