using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;


namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IHostEnvironment _webHost;

        public FileServices
            (
                ShopTARpe25Context context,
                IHostEnvironment webHost
            )
        {
            _context = context;
            _webHost = webHost;
        }

        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            //kindlasti peab ankeedil olema üks fail
            if (dto.Files != null && dto.Files.Count > 0)
            {
                //kui ei ole wwwroot-s multipleFileUpload directoryt
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    //, siis tee directory wwwrooti alla
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    //meil on vaja teha muutuja nimega uploadsFolder.
                    //sinna muutuja taha on vaja Path kombineerida
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    //igale failile unikaalne Guid selle nime ette
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    //iga kord, kui faili laed ülesse, siis tehakse see väikesteks 
                    //tükkideks
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        //domaini teha FileToApi
                        FileToApi path = new FileToApi
                        {
                            //tuleb ära mappida 
                            //domain ja ??
                            Id = Guid.NewGuid(),
                            ExistingFilePath = uniqueFileName,
                            SpaceshipId = domain.Id
                        };

                        _context.FileToApis.AddAsync(path);
                    }
                }
            }
        }

        public async Task<FileToApi> RemoveImageFromApi(FileToApiDto dto)
        {
            //kui on vaja kustutada fail, siis tuleb see üles leida
            var imageId = await _context.FileToApis
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            //kus asuvad failid, mida hakatakse kustutama
            var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                + imageId.ExistingFilePath;

            //kui fail on olemas, siis kustuta see
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            _context.FileToApis.Remove(imageId);
            await _context.SaveChangesAsync();

            return imageId;
        }

        public async Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dto)
        {
            foreach (var image in dto)
            {
                var imageId = await _context.FileToApis
                    .FirstOrDefaultAsync(x => x.Id == image.Id);

                var filePath = _webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"
                    + imageId.ExistingFilePath;

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                _context.FileToApis.Remove(imageId);
                await _context.SaveChangesAsync();
            }

            return null;
        }

        public void UploadFilesToDatabase(RealEstateDto dto, RealEstate domain)
        {
            //toimub kontroll, kas vähemalt üks fail või mitu
            if (dto.Files != null && dto.Files.Count > 0)
            {
                //tuleb kasutada foreachi, et mitu faili ülesse laadida
                foreach (var file in dto.Files)
                {
                    //foreachi sees tuleb kasutada using-t
                    using (var target = new MemoryStream())
                    {
                        //FileToDatabase
                        FileToDatabase files = new FileToDatabase()
                        {
                            Id = Guid.NewGuid(),
                            ImageTitle = file.FileName,
                            RealEstateId = domain.Id,
                        };

                        file.CopyTo(target);
                        files.ImageData = target.ToArray();

                        _context.FileToDatabases.Add(files);
                    }
                }
            }
        }

        public async Task<FileToDatabase> RemoveImageFromDatabase(FileToDatabaseDto dto)
        {
            var image = await _context.FileToDatabases
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            _context.FileToDatabases.Remove(image);
            await _context.SaveChangesAsync();

            return image;
        }

        //kui kustutad mitu pilti, siis peab olema massiivis ehk []
        public async Task<FileToDatabase> RemoveImagesFromDatabase(FileToDatabaseDto[] dtos)
        {
            foreach (var dto in dtos)
            {
                var image = await _context.FileToDatabases
                    .Where(x => x.Id == dto.Id)
                    .FirstOrDefaultAsync(x => x.Id == dto.Id);

                _context.FileToDatabases.Remove(image);
                await _context.SaveChangesAsync();
            }

            return null;
        }
    }
}