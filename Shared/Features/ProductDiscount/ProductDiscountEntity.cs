using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Features
{
	public class ProductDiscountEntity
	{
		[Column("id")]
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }

		[Column("product_id")]
		public long ProductId { get; set; }

		[Column("count")]
		public int Count { get; set; }

		[Column("percent")]
		public short Percent {  get; set; }

		[Column("price")]
		public decimal Price { get; set; }
	}
}
