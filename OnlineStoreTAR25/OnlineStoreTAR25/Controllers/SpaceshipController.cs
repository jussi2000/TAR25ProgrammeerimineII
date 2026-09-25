using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Spaceship;
using System.Reflection.Metadata.Ecma335;

namespace ShopTAR25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly IspaceshipServices _spaceshipService;
        private readonly ShopTARpe25Context _context;

        //teha constructor et saaks kasutada teenust, mis on
        //defineeritud IspaceshipServices liideses

        //lisage context juurde
        public SpaceshipController
            (
                IspaceshipServices ispaceshipService,
                ShopTARpe25Context context

            )
        {
            _spaceshipService = ispaceshipService;
            _context = context;
        }


        public IActionResult Index()
        {
            //loome vaheinstantsi domaini ja viewModeli vahel.
            var result = _context.Spaceships
                .Select(x => new SpaceshipIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    TeacherName = x.TeacherName,
                }).ToList(); // <-- See laeb andmed andmebaasist reaalselt sisse

            return View(result); // <-- See saadab andmed Index.cshtml failile
        }

        [HttpGet]
        //kui kasutaja klikib "Create" nuppu, siis see meetod käivitatakse
        //tagastab kasutajale vormi, kuhu saab sisestada andmed

        public IActionResult Create()
        {
            return View();
        }
        //kui oled teinud vormi, siis see meetod käivitatakse
        //saadab andmed serverisse, kus need salvestatakse andmebaasi

        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            //luua vaheinstants, mis sisaldab andmeid, mis on saadud vormist
            //need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamis

            var dto = new SpaceshipDto
            {
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
            };

            //kutsuda teenuse meetodit, mis salvestab andmed andmebaasi
            var result = await _spaceshipService.Create(dto);

            return RedirectToAction(nameof(Index));
        }
        //tuleb teha details meetod
        //see kutsub välja interfacest service meetodi

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {

            var spaceship = await _spaceshipService.DetailsAsync(id);

            //veakäsitlus
            //suunab vatele NotFound, kui andmed ei ole
            if (spaceship == null)
            {
                return NotFound();
            }

            //tuleb teha viewmodel ja see siin välja kutsuda
            //ära map'ida vm ja doamin

            var vm = new SpaceshipDetailsViewModel();

            vm.Id = spaceship.Id;
            vm.GroupName = spaceship.GroupName;
            vm.ChildrenCount = spaceship.ChildrenCount;
            vm.KindergartenName = spaceship.KindergartenName;
            vm.TeacherName = spaceship.TeacherName;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.UpdatedAt = spaceship.UpdatedAt;

            return View(vm);
        }
        //UPDATE ------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {

            var spaceship = await _spaceshipService.DetailsAsync(id);

            //veakäsitlus
            //suunab vatele NotFound, kui andmed ei ole
            if (spaceship == null)
            {
                return NotFound();
            }

            //tuleb teha viewmodel ja see siin välja kutsuda
            //ära map'ida vm ja doamin

            var vm = new SpaceshipUpdateViewModel();

            vm.Id = spaceship.Id;
            vm.GroupName = spaceship.GroupName;
            vm.ChildrenCount = spaceship.ChildrenCount;
            vm.KindergartenName = spaceship.KindergartenName;
            vm.TeacherName = spaceship.TeacherName;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.UpdatedAt = spaceship.UpdatedAt;

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(SpaceshipUpdateViewModel vm)
        {
            var dto = new SpaceshipDto()
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                TeacherName = vm.TeacherName,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt
            };
            var result = await _spaceshipService.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));

        }

        //DELETE -----------------------------------------------
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.GroupName = spaceship.GroupName;
            vm.ChildrenCount = spaceship.ChildrenCount;
            vm.KindergartenName = spaceship.KindergartenName;
            vm.TeacherName = spaceship.TeacherName;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.UpdatedAt = spaceship.UpdatedAt;

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var result = await _spaceshipService.Delete(id);

            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));

        }

    }
}
