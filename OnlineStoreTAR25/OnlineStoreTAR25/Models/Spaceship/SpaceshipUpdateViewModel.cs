namespace ShopTARpe25.Models.Spaceship
{
    public class SpaceshipUpdateViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Classification { get; set; } = string.Empty;
        public DateTime? BuildDate { get; set; }
        public int? Crew { get; set; }
        public int? EnginePower { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
