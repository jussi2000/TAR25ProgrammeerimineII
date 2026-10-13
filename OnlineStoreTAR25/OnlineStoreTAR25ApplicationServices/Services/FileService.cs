
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileService : IFileServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IHostEnvironment _environment;

        public FileService(
            ShopTARpe25Context context,
            IHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public void FilesToApi(KindergardenDto dto, Kindergarden domain)
        {
            if (dto.Files == null || dto.Files.Count == 0)
            {
                return;
            }

            var uploadsFolder = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot",
                "multipleFileUpload");

            Directory.CreateDirectory(uploadsFolder);

            foreach (var file in dto.Files)
            {
                if (file.Length <= 0)
                {
                    continue;
                }

                var uniqueFileName =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(file.FileName);

                var filePath = Path.Combine(
                    uploadsFolder,
                    uniqueFileName);

                using (var fileStream =
                       new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                var image = new FileToApi
                {
                    Id = Guid.NewGuid(),
                    ExistingFilePath = uniqueFileName,
                    KindergardenId = domain.Id
                };

                _context.FileToApis.Add(image);
            }
        }

        public async Task<FileToApi?> RemoveImageFromApi(
            FileToApiDto dto)
        {
            var image = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (image == null)
            {
                return null;
            }

            var filePath = Path.Combine(
                _environment.ContentRootPath,
                "wwwroot",
                "multipleFileUpload",
                image.ExistingFilePath ?? string.Empty);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.FileToApis.Remove(image);
            await _context.SaveChangesAsync();

            return image;
        }

        public async Task<List<FileToApi>> RemoveImagesFromApi(
            FileToApiDto[] dto)
        {
            var removedImages = new List<FileToApi>();

            foreach (var item in dto)
            {
                var image = await _context.FileToApis
                    .FirstOrDefaultAsync(x => x.Id == item.Id);

                if (image == null)
                {
                    continue;
                }

                var filePath = Path.Combine(
                    _environment.ContentRootPath,
                    "wwwroot",
                    "multipleFileUpload",
                    image.ExistingFilePath ?? string.Empty);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _context.FileToApis.Remove(image);
                removedImages.Add(image);
            }

            await _context.SaveChangesAsync();

            return removedImages;
        }
    }
}
