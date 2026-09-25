namespace ShopTARpe25.Models.Spaceship
{
    public class SpaceshipUpdateViewModel
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public int? ChildrenCount { get; set; }
        public string KindergartenName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
