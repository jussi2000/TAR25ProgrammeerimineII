using System;

namespace ShopTARpe25.Core.Domain
{
    public class RealEstate
    {
        public Guid Id { get; set; }
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ChildrenCount { get; set; }
        public string KindergartenName { get; set; }
        public string TeacherName { get; set; }

    }
}