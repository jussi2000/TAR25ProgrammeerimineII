using ShopTARpe25.Models.Kindergarden;
using System;
using System.Collections.Generic;

namespace ShopTARpe25.Models.Kindergarden
{
    public class KindergardenDeleteViewModel
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public int? ChildrenCount { get; set; }
        public string KindergartenName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<FileToApiViewModel> FilesToApiViewModels { get; set; } = new List<FileToApiViewModel>();
    }
}