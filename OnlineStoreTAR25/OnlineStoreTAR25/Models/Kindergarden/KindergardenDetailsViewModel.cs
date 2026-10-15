namespace ShopTARpe25.Models.Kindergarden
{
    public class KindergardenDetailsViewModel
    {
        public Guid Id { get; set; }
        public string GroupName { get; set; }
        public int? ChildrenCount { get; set; }
        public string KindergartenName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public List<FileToApiViewModel> FilesToApiViewModels { get; set; } = new List<FileToApiViewModel>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}