using Shared.Features;
using System.ComponentModel.DataAnnotations;

namespace Shared;

public partial class LocaleEntity
{
    [Key]
    public string Code { get; set; } = null!;
    public virtual ICollection<ProductEntity> ProductEntity { get; set; } = new List<ProductEntity>();
}
