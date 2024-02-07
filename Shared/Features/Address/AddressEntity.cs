using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
    [Table("addresses")]
    public class AddressEntity
    {
        [Column("id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("region")]
        public string Region { get; set; } = null!;

        [Column("district")]
        public string District { get; set; } = null!;

        [Column("street")]
        public string Street { get; set; } = null!;

        [Column("home_number")]
        public string HomeNumber { get; set; } = null!;

        [Column("home_or_office")]
        public int HomeOrOffice { get; set; }

        [Column("domophone_code")]
        public string? DomophoneCode { get; set; }

        //Relations
        public virtual UserEntity? User { get; set; }
    }
}
