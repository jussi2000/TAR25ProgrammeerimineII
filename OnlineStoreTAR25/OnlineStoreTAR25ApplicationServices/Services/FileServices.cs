using Microsoft.Extensions.Hosting;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;
using System.Net;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        //teha cunstructor, mis ühendab DB-d
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
        // See public FileServices on constructor
       public void FilesToApi(SpaceshipDto dto, Spaceship domain)
       {
            //kindlasti peab ankeedil olema üks fail 
            if(dto.Files != null && dto.Files.Count > 0)
            {
                //kui ei ole wwwroot'is multipleFileUpload directoryt
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    //, siis tee directory wwwrooti alla
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    //meil on vaja teha muutuja nimega uploadsFolder
                    //sinna muutuja taha on vaja Path kombineeria
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    //igale failile unikaalne Guid selle nime ette
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        //domaini teha FileToApi
                        FileToApi path = new FileToApi
                        {
                            //tuleb ära mappida
                            //domain ja ??
                            Id = Guid.NewGuid(),
                            ExistingFilePath =  uniqueFileName,
                            SpaceshipId = domain.Id
                        };

                        _context.FileToApis.AddAsync(path);


                    }
                }
            }
       }
    }
}
