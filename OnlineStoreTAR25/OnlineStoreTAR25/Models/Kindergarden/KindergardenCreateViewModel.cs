using System;

namespace ShopTARpe25.Models.Kindergarden
{
    public class KindergardenCreateViewModel
    {
        public string GroupName { get; set; }
        public int ChildrenCount { get; set; }
        public string KindergartenName { get; set; }
        public string TeacherName { get; set; }
        public List<Microsoft.AspNetCore.Http.IFormFile> Files { get; set; }

    }
}