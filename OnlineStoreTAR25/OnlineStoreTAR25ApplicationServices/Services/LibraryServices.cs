using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Client;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{

    public class LibraryServices : ILibraryServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileServices;

        public LibraryServices
            (
                ShopTARpe25Context context,
                IFileServices fileServices
            )
        {
            _context = context;
            _fileServices = fileServices;
        }
        public async Task<Library> Create(LibraryDto dto)
        {

            Library domain = new();

            domain.Id = Guid.NewGuid();
            domain.Name = dto.Name;
            domain.Classification = dto.Classification;
            domain.BuildDate = dto.BuildDate;
            domain.Crew = dto.Crew;
            domain.EnginePower = dto.EnginePower;
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;
            //peame saama File teenuse välja kutsuda,
            //mis salvestab failid serverisse
            _fileServices.FilesToApi(dto, domain);


            //siia tuleb kood, mis salvestab domain
            //objecti andmebaasi
            //tuleb kasutada repository'd, mis on
            //defineeritud Core projectis
            //konstruktori kaudu tuleb injectida repository

            await _context.Libraries.AddAsync(domain);
            await _context.SaveChangesAsync();


            return domain;
        }

        //siia teha uus meetod nimega DetailAsync
        //see ainult pärib andmed contextist
        public async Task<Library> DetailsAsync(Guid id) //otsitakse läbi id ehk tuleb Guid id panna
        {
            var result = await _context.Libraries
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;

        }

        public async Task<Library> Update(LibraryDto dto)
        {
            Library library = new();

            library.Id = dto.Id;
            library.Name = dto.Name;
            library.Classification = dto.Classification;
            library.BuildDate = dto.BuildDate;
            library.Crew = dto.Crew;
            library.EnginePower = dto.EnginePower;
            library.CreatedAt = dto.CreatedAt;
            library.ModifiedAt = DateTime.Now;
            _fileServices.FilesToApi(dto, library);

            _context.Libraries.Update(library);
            await _context.SaveChangesAsync();

            return library;
        }
        //DELETE ---------------------------------------------------
        public async Task<Library> Delete(Guid id)
        {
            var result = await _context.Libraries
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToApis
                 .Where(x => x.LibraryId == id)
             .Select(y => new FileToApiDto
             {
                 Id = y.Id,
                 LibraryId = y.LibraryId,
                 ExistingFilePath = y.ExistingFilePath,

             }).ToArrayAsync();

            //peame kustutama eilse, mis kustutab failid serverist
            await _fileServices.RemoveImagesFromApi(images);

            _context.Libraries.Remove(result);

            _context.Libraries.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
