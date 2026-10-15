
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;

namespace ShopTARpe25.ApplicationServices.Services
{
    public class KindergardenServices : IKindergardenServices
    {
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileServices;

        public KindergardenServices(
            ShopTARpe25Context context,
            IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<Kindergarden> Create(KindergardenDto dto)
        {
            Kindergarden domain = new()
            {
                Id = Guid.NewGuid(),
                GroupName = dto.GroupName,
                ChildrenCount = dto.ChildrenCount,
                KindergartenName = dto.KindergartenName,
                TeacherName = dto.TeacherName,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _fileServices.FilesToApi(dto, domain);
            await _context.Kindergardens.AddAsync(domain);
            await _context.SaveChangesAsync();
            return domain;
        }

        public async Task<Kindergarden?> DetailsAsync(Guid id)
        {
            return await _context.Kindergardens
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Kindergarden> Update(KindergardenDto dto)
        {
            var domain = await _context.Kindergardens
                .FirstOrDefaultAsync(x => x.Id == dto.Id);

            if (domain == null)
            {
                return null!;
            }

            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.UpdatedAt = DateTime.Now;

            _fileServices.FilesToApi(dto, domain);
            await _context.SaveChangesAsync();
            return domain;
        }

        public async Task<Kindergarden?> Delete(Guid id)
        {
            var domain = await _context.Kindergardens
                .FirstOrDefaultAsync(x => x.Id == id);

            if (domain == null)
            {
                return null;
            }

            var images = await _context.FileToApis
                .Where(x => x.KindergardenId == id)
                .Select(x => new FileToApiDto
                {
                    Id = x.Id,
                    ExistingFilePath = x.ExistingFilePath,
                    KindergardenId = x.KindergardenId
                })
                .ToArrayAsync();

            await _fileServices.RemoveImagesFromApi(images);
            _context.Kindergardens.Remove(domain);
            await _context.SaveChangesAsync();
            return domain;
        }
    }
}
