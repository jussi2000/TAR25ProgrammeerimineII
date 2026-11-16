namespace ShopTARpe25.Models.RealEstate
{
    public class RealEstateCreateUpdateViewModel
    {
        public Guid? Id { get; set; }
        public double? Area { get; set; }
        public string? Address { get; set; }
        public int RoomNumber { get; set; }
        public string? BuildingType { get; set; }

        public List<IFormFile>? Files { get; set; }
        public List<RealEstateImageViewModel> Image { get; set; }
            = [];

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}