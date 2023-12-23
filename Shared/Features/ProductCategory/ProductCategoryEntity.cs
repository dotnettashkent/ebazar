using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
	public class ProductCategoryEntity
	{
		[Column("id")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[Column("locale")]
		public string Locale { get; set; } = null!;

		[Column("main_name")]
		public string MainName { get; set; } = null!;

		[Column ("main_link")]
		public string MainLink { get; set;} = null!;

		[Column("second_name")]
		public string SecondName { get; set; } = null!;

		[Column("second_link")]
		public string SecondLink { get; set; } = null!;

		[Column("third_name")]
		public string ThirdName { get; set;} = null!;

		[Column("third_link")]
		public string ThirdLink { get; set;} = null!;

		[Column("is_popular")]
		public bool IsPopular { get; set; }

		[Column("photo")]
		public FileEntity? Photo { get; set; }

		[Column("photo_mobile")]
		public FileEntity? PhotoMobile { get; set;}
	}
}
