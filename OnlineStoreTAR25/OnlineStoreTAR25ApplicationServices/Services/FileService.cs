using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopTARpe25Context _context;

        public FileServices(ShopTARpe25Context context)
        {
            _context = context;
        }

        public async Task<FileToApi> UploadFilesToApi(FileToApiDto dto)
        {
            if (dto.File != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "multipleFileUpload");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + dto.File.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(fileStream);
                }

                var domain = new FileToApi
                {
                    Id = Guid.NewGuid(),
                    FileUrl = uniqueFileName,
                    RealEstateId = dto.RealEstateId
                };

                await _context.FilesToApi.AddAsync(domain);
                await _context.SaveChangesAsync();

                return domain;
            }

            return null;
        }

        public async Task<FileToApi> RemoveImage(FileToApiDto dto)
        {
            return null;
        }
    }
}