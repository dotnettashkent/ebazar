using MemoryPack;
using Shared.Infrastructures;

namespace Shared.Features.File
{
	[MemoryPackable]
	public partial class FileEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public Guid? FileId { get; set; }
        public string? Extension { get; set; }
        public string? Path { get; set; }
        public long Size { get; set; } = 0;
        public UFileTypes Type { get; set; } = UFileTypes.File;
    }
}
