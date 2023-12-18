using Shared.Features.File;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features.ProductCategory
{
	public class ProductCategoryEntity
	{
		[Column("id")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[Column("main_name")]
		public string MainName { get; set; } = string.Empty;

		[Column ("main_link")]
		public string MainLink { get; set;} = string.Empty;

		[Column("second_name")]
		public string SecondName { get; set; } = string.Empty;

		[Column("second_link")]
		public string SecondLink { get; set; } = string.Empty;

		[Column("third_name")]
		public string ThirdName { get; set;} = string.Empty;

		[Column("third_link")]
		public string ThirdLink { get; set;} = string.Empty;

		[Column("photo")]
		public FileEntity? Photo { get; set; }
	}
}
