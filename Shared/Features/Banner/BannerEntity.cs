using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
    [Table("banners")]
    public class BannerEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("link")]
        public string Link { get; set; } = null!;

        [Column("sort")]
        public int Sort { get; set; }

        [Column("photo")]
        public string? Photo { get; set; }
    }
}
