using Shared.Features.File;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Features.Brands
{
	public class BrandEntity
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("id")]
		public long Id { get; set; }

		[Column("name")]
		public string Name { get; set; } = null!;

		[Column("is_popular")]
		public string IsPopular { get; set; } = null!;

		[Column("photo_id")]
		public FileEntity? Photo { get; set; }
	}
}
