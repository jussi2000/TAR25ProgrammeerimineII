using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.RealEstate;

namespace ShopTARpe25.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly ShopTARpe25Context _context;
        private readonly IRealEstateServices _realEstateServices;

        public RealEstateController(
            ShopTARpe25Context context,
            IRealEstateServices realEstateServices)
        {
            _context = context;
            _realEstateServices = realEstateServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _realEstateServices.Index()
                .Select(x => new RealEstateIndexViewModel
                {
                    Id = x.Id,
                    Address = x.Address,
                    Area = x.Area,
                    RoomNumber = x.RoomNumber,
                    BuildingType = x.BuildingType

                });

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _realEstateServices.DetailAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            var vm = new RealEstateDetailsViewModel
            {
                Id = result.Id,
                Area = result.Area,
                Address = result.Address,
                RoomNumber = result.RoomNumber,
                BuildingType = result.BuildingType,
                CreatedAt = result.CreatedAt,
                ModifiedAt = result.ModifiedAt
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel model = new();
            return View("CreateUpdate", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel model)
        {
            var dto = new RealEstateDto()
            {
                Area = model.Area,
                Address = model.Address,
                RoomNumber = model.RoomNumber,
                BuildingType = model.BuildingType,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt
            };

            await _realEstateServices.Create(dto);

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

            var vm = new RealEstateCreateUpdateViewModel
            {
                Id = result.Id,
                Area = result.Area,
                Address = result.Address,
                RoomNumber = result.RoomNumber,
                BuildingType = result.BuildingType,
                CreatedAt = result.CreatedAt,
                ModifiedAt = result.ModifiedAt
            };

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel model)
        {
            var dto = new RealEstateDto()
            {
                Id = model.Id,
                Area = model.Area,
                Address = model.Address,
                RoomNumber = model.RoomNumber,
                BuildingType = model.BuildingType,
                CreatedAt = model.CreatedAt,
                ModifiedAt = model.ModifiedAt
            };

            await _realEstateServices.Update(dto);

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
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var result = await _realEstateServices.Delete(id);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}