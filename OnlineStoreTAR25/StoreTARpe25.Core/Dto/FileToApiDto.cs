using Microsoft.AspNetCore.Http;
using System;

namespace ShopTARpe25.Core.Dto
{
    public class FileToApiDto
    {
        public Guid Id { get; set; }
        public string FileUrl { get; set; }
        public Guid RealEstateId { get; set; }
        public IFormFile File { get; set; }
    }
}