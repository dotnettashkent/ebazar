using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Features
{
	[Table("brands")]
	public class BrandEntity
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("id")]
		public long Id { get; set; }

		[Column("name")]
		public string Name { get; set; } = null!;

		[Column("is_popular")]
		public string IsPopular { get; set; } = null!;

		public string Link { get; set; } = null!;

		/*[Column("photo_id")]
		public FileEntity? Photo { get; set; }*/
	}
}
