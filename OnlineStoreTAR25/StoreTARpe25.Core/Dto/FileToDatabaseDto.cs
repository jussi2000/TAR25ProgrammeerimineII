namespace ShopTARpe25.Core.Dto
{
    public class FileToDatabaseDto
    {
        public Guid? Id { get; set; }
        public string? ImageTitle { get; set; }
        public byte[]? ImageData { get; set; }
        public Guid? RealEstateId { get; set; }
    }
}
