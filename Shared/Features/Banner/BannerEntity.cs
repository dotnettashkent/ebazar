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

        [Column("locale")]
        public string Locale { get; set; } = null!;

        [Column("photo_id")]
        public string Photo { get; set; } = null!;
        
        [Column("title")]
        public string Title { get; set; } = null!;

        [Column("link")]
        public string Link { get; set; } = null!;

        [Column("description")]
        public string Description { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get;set; }
    }
}
