
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Spaceship;

namespace ShopTAR25.Controllers
{
    public class KindergardenController : Controller
    {
        private readonly IKindergardenServices _kindergardenService;
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileService;

        public KindergardenController(
            IKindergardenServices kindergardenService,
            ShopTARpe25Context context,
            IFileServices fileService)
        {
            _kindergardenService = kindergardenService;
            _context = context;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            var result = _context.Kindergardens
                .Select(x => new KindergardenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    TeacherName = x.TeacherName
                }).ToList();

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new KindergardenCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            KindergardenCreateViewModel vm)
        {
            var dto = new KindergardenDto
            {
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                Files = vm.Files ?? new List<Microsoft.AspNetCore.Http.IFormFile>()
            };

            await _kindergardenService.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarden =
                await _kindergardenService.DetailsAsync(id);

            if (kindergarden == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.KindergardenId == id)
                .Select(x => new ImageViewModel
                {
                    FilePath = x.ExistingFilePath,
                    ImageId = x.Id,
                    KindergardenId = x.KindergardenId
                }).ToArrayAsync();

            var vm = new KindergardenDetailsViewModel
            {
                Id = kindergarden.Id,
                GroupName = kindergarden.GroupName,
                ChildrenCount = kindergarden.ChildrenCount,
                KindergartenName = kindergarden.KindergartenName,
                TeacherName = kindergarden.TeacherName,
                CreatedAt = kindergarden.CreatedAt,
                UpdatedAt = kindergarden.UpdatedAt
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarden =
                await _kindergardenService.DetailsAsync(id);

            if (kindergarden == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.KindergardenId == id)
                .Select(x => new ImageViewModel
                {
                    FilePath = x.ExistingFilePath,
                    ImageId = x.Id,
                    KindergardenId = x.KindergardenId
                }).ToArrayAsync();

            var vm = new KindergardenUpdateViewModel
            {
                Id = kindergarden.Id,
                GroupName = kindergarden.GroupName,
                ChildrenCount = kindergarden.ChildrenCount,
                KindergartenName = kindergarden.KindergartenName,
                TeacherName = kindergarden.TeacherName,
                CreatedAt = kindergarden.CreatedAt,
                UpdatedAt = kindergarden.UpdatedAt
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(
            KindergardenUpdateViewModel vm)
        {
            var dto = new KindergardenDto
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt,
                Files = vm.Files ?? new List<Microsoft.AspNetCore.Http.IFormFile>(),
                FileToApiDtos = vm.Images
                    .Select(image => new FileToApiDto
                    {
                        Id = image.ImageId,
                        ExistingFilePath = image.FilePath,
                        KindergardenId = image.KindergardenId
                    }).ToArray()
            };

            await _kindergardenService.Update(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarden =
                await _kindergardenService.DetailsAsync(id);

            if (kindergarden == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.KindergardenId == id)
                .Select(x => new ImageViewModel
                {
                    FilePath = x.ExistingFilePath,
                    ImageId = x.Id,
                    KindergardenId = x.KindergardenId
                }).ToArrayAsync();

            var vm = new KindergardenDeleteViewModel
            {
                Id = kindergarden.Id,
                GroupName = kindergarden.GroupName,
                ChildrenCount = kindergarden.ChildrenCount,
                KindergartenName = kindergarden.KindergartenName,
                TeacherName = kindergarden.TeacherName,
                CreatedAt = kindergarden.CreatedAt,
                UpdatedAt = kindergarden.UpdatedAt
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            await _kindergardenService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(
            Guid imageId,
            Guid kindergardenId)
        {
            await _fileService.RemoveImageFromApi(
                new FileToApiDto
                {
                    Id = imageId
                });

            return RedirectToAction(
                nameof(Update),
                new { id = kindergardenId });
        }
    }
}