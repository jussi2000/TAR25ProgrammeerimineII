using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Models.Spaceship;
using System.Reflection.Metadata.Ecma335;

namespace ShopTAR25.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly IspaceshipServices _spaceshipService;


        //teha constructor et saaks kasutada teenust, mis on
        //defineeritud IspaceshipServices liideses

        public SpaceshipController
            (
                IspaceshipServices ispaceshipService
            )
        {
            _spaceshipService = ispaceshipService;  
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //kui kasutaja klikib "Create" nuppu, siis see meetod käivitatakse
        //tagastab kasutajale vormi, kuhu saab sisestada andmed

        public IActionResult create()
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
                Name = vm.Name,
                Classification = vm.Classification,
                BuildDate = vm.BuildDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
            };

            //kutsuda teenuse meetodit, mis salvestab andmed andmebaasi
            var result = await _spaceshipService.Create(dto);

            return RedirectToAction(nameof(Index));
        }

    }
}
