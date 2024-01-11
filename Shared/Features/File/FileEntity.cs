using MemoryPack;
using Shared.Infrastructures;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Features
{
    [MemoryPackable]
    [Table("files")]
	public partial class FileEntity : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; } = null!;
        
        [Column("file_id")]
        public Guid? FileId { get; set; }

        [Column("extension")]
        public string? Extension { get; set; }

        [Column("path")]
        public string? Path { get; set; }

        [Column("size")]
        public long Size { get; set; } = 0;

        [Column("type")]
        public UFileTypes Type { get; set; } = UFileTypes.File;
    }
}
