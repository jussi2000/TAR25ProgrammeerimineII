using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;


namespace ShopTARpe25.ApplicationServices.Services
{
    public class RealEstateServices : IRealEstateServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileServices;

        public RealEstateServices
            (
                ShopTARpe25Context context,
                IFileServices fileServices
            )
        {
            _context = context;
            _fileServices = fileServices;
        }


        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate domain = new RealEstate();

            domain.Id = Guid.NewGuid();
            domain.Area = dto.Area;
            domain.Address = dto.Address;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            //kui ankeedis on pildid, siis kutsu faili salvestamine v'lja
            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, domain);
            }

            await _context.RealEstates.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            RealEstate domain = new RealEstate();

            domain.Id = dto.Id;
            domain.Area = dto.Area;
            domain.Address = dto.Address;
            domain.RoomNumber = dto.RoomNumber;
            domain.BuildingType = dto.BuildingType;
            domain.CreatedAt = dto.CreatedAt;
            domain.ModifiedAt = DateTime.Now;

            if(dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, domain);
            }

            _context.RealEstates.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<RealEstate> DetailAsync(Guid id)
        {
            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<RealEstate> Delete(Guid id)
        {
            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new FileToDatabaseDto
                {
                    Id = y.Id,
                    ImageTitle = y.ImageTitle,
                    RealEstateId = y.RealEstateId,
                }).ToArrayAsync();

            await _context.RealEstates
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();

            await _context.SaveChangesAsync();

            return result;
        }
    }
}