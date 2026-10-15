
using Microsoft.AspNetCore.Http;

namespace ShopTARpe25.Core.Dto
{
    public class KindergardenDto
    {
        public Guid Id { get; set; }

        public string GroupName { get; set; } = string.Empty;

        public int? ChildrenCount { get; set; }

        public string KindergartenName { get; set; } = string.Empty;

        public string TeacherName { get; set; } = string.Empty;

        public List<IFormFile> Files { get; set; } = new();

        public IEnumerable<FileToApiDto> FileToApiDtos { get; set; }
            = new List<FileToApiDto>();

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
