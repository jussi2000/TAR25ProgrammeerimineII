
using Microsoft.AspNetCore.Http;

namespace ShopTARpe25.Models.Spaceship
{
    public class KindergardenCreateViewModel
    {
        public string GroupName { get; set; } = string.Empty;
        public int? ChildrenCount { get; set; }
        public string KindergartenName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public List<IFormFile> Files { get; set; } = new();
    }
}
