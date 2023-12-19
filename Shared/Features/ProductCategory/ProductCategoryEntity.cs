using Shared.Features.File;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
	public class ProductCategoryEntity
	{
		[Column("id")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[Column("main_name_uz")]
		public string MainNameUz { get; set; } = null!;

		[Column("main_name_ru")]
		public string MainNameRu { get; set; } = null!;

		[Column("main_name_en")]
		public string MainNameEn { get; set; } = null!;

		[Column ("main_link_uz")]
		public string MainLinkUz { get; set;} = null!;

		[Column("main_link_ru")]
		public string MainLinkRu { get; set; } = null!;

		[Column("main_link_en")]
		public string MainLinkEn { get; set; } = null!;

		[Column("second_name_uz")]
		public string SecondNameUz { get; set; } = null!;

		[Column("second_name_ru")]
		public string SecondNameRu { get; set; } = null!;

		[Column("second_name_en")]
		public string SecondNameEn { get; set; } = null!;

		[Column("second_link_uz")]
		public string SecondLinkUz { get; set; } = null!;

		[Column("second_link_ru")]
		public string SecondLinkRu { get; set; } = null!;

		[Column("second_link_en")]
		public string SecondLinkEn { get; set; } = null!;

		[Column("third_name_uz")]
		public string ThirdNameUz { get; set;} = null!;

		[Column("third_name_ru")]
		public string ThirdNameRu { get; set; } = null!;

		[Column("third_name_en")]
		public string ThirdNameEn { get; set; } = null!;

		[Column("third_link_uz")]
		public string ThirdLinkUz { get; set;} = null!;

		[Column("third_link_ru")]
		public string ThirdLinkRu { get; set; } = null!;

		[Column("is_popular")]
		public bool IsPopular { get; set; }

		[Column("photo")]
		public FileEntity? Photo { get; set; }
	}
}
