using Microsoft.AspNetCore.Mvc;
using ShopTARpe25.Models.Spaceship;

namespace ShopTAR25.Controllers
{
    public class SpaceshipController : Controller
    {
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
            return RedirectToAction(nameof(Index));
        }
    }
}
