using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{

    public class SpaceshipServices : IspaceshipServices
    {
        private readonly ShopTARpe25Context _context;

        public SpaceshipServices
            (
                ShopTARpe25Context context
            )
        {
            _context = context;
        }
        public async Task<Spaceship> Create(SpaceshipDto dto)
        {

            Spaceship domain = new();

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

        //siia teha uus meetod nimega DetailAsync
        //see ainult pärib andmed contextist
        public async Task<Spaceship> DetailsAsync(Guid id) //otsitakse läbi id ehk tuleb Guid id panna
        {
            var result = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
    }
}
