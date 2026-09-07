using Microsoft.EntityFrameworkCore.Diagnostics;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{

    public class SpaceshipServices
    {
        private readonly ShopTARpe25Context _context;

        public SpaceshipServices
            (
                ShopTARpe25Context context
            )
        {
            _context = context;
        }
        public async Task<SpaceShip> Create(SpaceshipDto dto)
        {

            SpaceShip domain = new();

            domain.Id = dto.Id;
            domain.Name = dto.Name;
            domain.Classification = dto.Classification;
            domain.BuildDate = dto.BuildDate;
            domain.Crew = dto.Crew;
            domain.EnginePower = dto.EnginePower;
            domain.CreatedAt = dto.CreatedAt;
            domain.ModifiedAt = dto.ModifiedAt;

            //siia tuleb kood, mis salvestab domain
            //objecti andmebaasi
            //tuleb kasutada repository'd, mis on
            //defineeritud Core projectis
            //konstruktori kaudu tuleb injectida repository

            await _context.Spaceships.AddAsync(domain);
            await _context.SaveChangesAsync();


            return domain;
        }
    }
}
