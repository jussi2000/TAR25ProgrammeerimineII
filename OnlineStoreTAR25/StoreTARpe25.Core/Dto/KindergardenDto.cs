using System;

namespace ShopTARpe25.Core.Dto
{
    public class KindergardenDto
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public int ChildrenCount { get; set; }
        public string KindergartenName { get; set; }
        public string TeacherName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Microsoft.AspNetCore.Http.IFormFile> Files { get; set; }

    }
}