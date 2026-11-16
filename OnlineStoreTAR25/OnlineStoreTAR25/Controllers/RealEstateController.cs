using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.RealEstate;
using Microsoft.EntityFrameworkCore;


namespace ShopTARpe25.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly ShopTARpe25Context _context;
        private readonly IRealEstateServices _realEstateServices;

        public RealEstateController
            (
                ShopTARpe25Context context,
                IRealEstateServices realEstateServices
            )
        {
            _context = context;
            _realEstateServices = realEstateServices;
        }

        public IActionResult Index()
        {
            var result = _context.RealEstates
                .Select(x => new RealEstateIndexViewModel
                {
                    Id = x.Id,
                    Area = x.Area,
                    BuildingType = x.BuildingType,
                    RoomNumber = x.RoomNumber,
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel result = new();

            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUpdate", model);
            }

            var dto = new RealEstateDto()
            {
                Area = model.Area,
                Address = model.Address,
                RoomNumber = model.RoomNumber,
                BuildingType = model.BuildingType,
                Files = model.Files,
                Image = model.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.ImageId,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId
                    }).ToArray()
            };

            var result = await _realEstateServices.Create(dto);

            if (result == null)
            {
                ModelState.AddModelError("", "Real estate creation failed.");
                return View(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var result = await _realEstateServices.DetailAsync(id);

            if (result == null)
            {
                return NotFound();
            }

           RealEstateImageViewModel[] images = await FileFromDatabase(id);

            var vm = new RealEstateCreateUpdateViewModel();

            vm.Id = result.Id;
            vm.Area = result.Area;
            vm.Address = result.Address;
            vm.RoomNumber = result.RoomNumber;
            vm.BuildingType = result.BuildingType;
            vm.CreatedAt = result.CreatedAt;
            vm.ModifiedAt = result.ModifiedAt;
            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUpdate", vm);
            }

            var dto = new RealEstateDto
            {
                Id = vm.Id,
                Area = vm.Area,
                Address = vm.Address,
                RoomNumber = vm.RoomNumber,
                BuildingType = vm.BuildingType,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt
            };

            var result = await _realEstateServices.Update(dto);

            if (result == null)
            {
                ModelState.AddModelError("", "Real estate update failed.");
                return View(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _realEstateServices.DetailAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            var vm = new RealEstateDeleteViewModel
            {
                Id = result.Id,
                Area = result.Area,
                Address = result.Address,
                RoomNumber = result.RoomNumber,
                BuildingType = result.BuildingType,
                CreatedAt = result.CreatedAt,
                ModifiedAt = result.ModifiedAt,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var realEstate = await _realEstateServices.Delete(id);

            if (realEstate == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _realEstateServices.DetailAsync(id);

            if (result == null)
            {
                ModelState.AddModelError("", "Real estate detail failed.");
                return View(nameof(Index));
            }

            RealEstateImageViewModel[] images = await FileFromDatabase(id);

            //siin toimub info edasi liikumine dto-st viewModelini
            var vm = new RealEstateDetailsViewModel();

            vm.Id = result.Id;
            vm.Area = result.Area;
            vm.Address = result.Address;
            vm.RoomNumber = result.RoomNumber;
            vm.BuildingType = result.BuildingType;
            vm.CreatedAt = result.CreatedAt;
            vm.ModifiedAt = result.ModifiedAt;
            vm.Images.AddRange(images);

            return View(vm);
        }

        private async Task<RealEstateImageViewModel[]> FileFromDatabase(Guid id)
        {
            var result = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                    ImageId = y.Id,
                    RealEstateId = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    //string.Format tähendab e see muudab andmed string andmetüübiks
                    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();

            return result;
        }
    }
}