using Microsoft.AspNetCore.Http;
using ShopTARpe25.Core.Serviceinterface;

namespace ShopTARpe25.Core.Dto
{
    public class LibraryDto 
    {
        //Dto class on selleks et vahetada andemid
public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<IFormFile>? Files { get; set; } = new List<IFormFile>();
        public IEnumerable<FileToApiDto> FileToApiDtos { get; set; } = new List<FileToApiDto>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
