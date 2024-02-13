using Shared.Infrastructures;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Features
{
    [Table("project_users")]
    public class UserEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public long Id { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; } = null!;

        [Column("password")]
        public string Password { get; set; } = null!;

        [Column("role")]
        public string? Role { get; set; } = UserRole.User.ToString();

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        //Relations
        public virtual CartEntity? Cart { get; set; }
        public virtual FavouriteEntity? Favourite { get; set; }
        public virtual ICollection<OrderEntity>? Orders { get; set; } = new List<OrderEntity>();
        public virtual ICollection<AddressEntity> Addresses { get; set; } = new List<AddressEntity>();
    }
}
