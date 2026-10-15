using System;

namespace ShopTARpe25.Core.Domain
{
    public class FileToApi
    {
        public Guid Id { get; set; }
        public string FileUrl { get; set; } = string.Empty;
        public Guid RealEstateId { get; set; }
    }
}