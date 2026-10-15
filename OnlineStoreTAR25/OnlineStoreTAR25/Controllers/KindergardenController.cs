using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Kindergarden;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace ShopTARpe25.Controllers
{
    public class KindergardenController : Controller
    {
        private readonly IKindergardenServices _kindergardenService;
        private readonly ShopTARpe25Context _context;

        public KindergardenController
            (
                IKindergardenServices ikindergardenService,
                ShopTARpe25Context context
            )
        {
            _kindergardenService = ikindergardenService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var realEstatesData = await _context.RealEstates.ToListAsync();

            var result = realEstatesData.Select(x => new KindergardenIndexViewModel
            {
                Id = x.Id,
                GroupName = x.Address,
                ChildrenCount = x.ChildrenCount,
                KindergartenName = x.KindergartenName,
                TeacherName = x.TeacherName,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergardenCreateViewModel vm)
        {
            var dto = new KindergardenDto
            {
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                Files = vm.Files,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _kindergardenService.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var dbRealEstate = await _context.RealEstates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbRealEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FilesToApi
                .Where(x => x.RealEstateId == id)
                .Select(x => new FileToApiViewModel
                {
                    ImageId = x.Id,
                    FileUrl = x.FileUrl,
                    RealEstateId = x.RealEstateId
                }).ToListAsync();

            var vm = new KindergardenDetailsViewModel
            {
                Id = dbRealEstate.Id,
                GroupName = dbRealEstate.Address,
                ChildrenCount = dbRealEstate.ChildrenCount,
                KindergartenName = dbRealEstate.KindergartenName,
                TeacherName = dbRealEstate.TeacherName,
                CreatedAt = dbRealEstate.CreatedAt,
                UpdatedAt = dbRealEstate.UpdatedAt,
                FilesToApiViewModels = images
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var dbRealEstate = await _context.RealEstates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbRealEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FilesToApi
                .Where(x => x.RealEstateId == id)
                .Select(x => new FileToApiViewModel
                {
                    ImageId = x.Id,
                    FileUrl = x.FileUrl,
                    RealEstateId = x.RealEstateId
                }).ToListAsync();

            var vm = new KindergardenUpdateViewModel
            {
                Id = dbRealEstate.Id,
                GroupName = dbRealEstate.Address,
                ChildrenCount = dbRealEstate.ChildrenCount,
                KindergartenName = dbRealEstate.KindergartenName,
                TeacherName = dbRealEstate.TeacherName,
                CreatedAt = dbRealEstate.CreatedAt,
                UpdatedAt = dbRealEstate.UpdatedAt,
                FilesToApiViewModels = images
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergardenUpdateViewModel vm)
        {
            var dto = new KindergardenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount ?? 0,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                Files = vm.Files,
                CreatedAt = vm.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _kindergardenService.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var dbRealEstate = await _context.RealEstates.FirstOrDefaultAsync(x => x.Id == id);

            if (dbRealEstate == null)
            {
                return NotFound();
            }

            var images = await _context.FilesToApi
                .Where(x => x.RealEstateId == id)
                .Select(x => new FileToApiViewModel
                {
                    ImageId = x.Id,
                    FileUrl = x.FileUrl,
                    RealEstateId = x.RealEstateId
                }).ToListAsync();

            var vm = new KindergardenDeleteViewModel
            {
                Id = dbRealEstate.Id,
                GroupName = dbRealEstate.Address,
                ChildrenCount = dbRealEstate.ChildrenCount,
                KindergartenName = dbRealEstate.KindergartenName,
                TeacherName = dbRealEstate.TeacherName,
                CreatedAt = dbRealEstate.CreatedAt,
                UpdatedAt = dbRealEstate.UpdatedAt,
                FilesToApiViewModels = images
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            await _kindergardenService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveImage(Guid imageId, Guid kindergardenId)
        {
            var image = await _context.FilesToApi.FirstOrDefaultAsync(x => x.Id == imageId);

            if (image != null)
            {
                _context.FilesToApi.Remove(image);
                await _context.SaveChangesAsync();

                string uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "multipleFileUpload");
                string path = Path.Combine(uploadDir, image.FileUrl);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }

            return RedirectToAction(nameof(Update), new { id = kindergardenId });
        }
    }
}