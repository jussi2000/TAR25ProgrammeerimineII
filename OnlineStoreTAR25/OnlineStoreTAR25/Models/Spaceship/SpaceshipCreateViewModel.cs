namespace ShopTARpe25.Models.Spaceship
{
    public class SpaceshipCreateViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Classification { get; set; } = string.Empty;
        public DateTime? BuildDate { get; set; }
        public int? Crew { get; set; }
        public int? EnginePower { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<imageViewModel> Image { get; set; }
            = new List<imageViewModel>();
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
