using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Library;
using ShopTARpe25.Models.Library;
using System.Reflection.Metadata.Ecma335;

namespace ShopTAR25.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILibraryServices _libraryService;
        private readonly ShopTARpe25Context _context;
        private readonly IFileServices _fileService;

        //teha constructor et saaks kasutada teenust, mis on
        //defineeritud IlibraryServices liideses

        //lisage context juurde
        public LibraryController
            (
                ILibraryServices ilibraryService,
                ShopTARpe25Context context,
               IFileServices fileService

            )
        {
            _libraryService = ilibraryService;
            _context = context;
            _fileService = fileService;
        }


        public IActionResult Index()
        {
            //loome vaheinstantsi domaini ja viewModeli vahel.
            var result = _context.Libraries
                .Select(x => new LibraryIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Classification = x.Classification,
                    BuildDate = x.BuildDate,
                    Crew = x.Crew,
                    EnginePower = x.EnginePower
                });

            return View(result); // <-- See saadab andmed Index.cshtml failile
        }

        [HttpGet]
        //kui kasutaja klikib "Create" nuppu, siis see meetod käivitatakse
        //tagastab kasutajale vormi, kuhu saab sisestada andmed

        public IActionResult Create()
        {
            LibraryCreateUpdateViewModel vm = new();

            return View("CreateUpdate", vm);
        }
        //kui oled teinud vormi, siis see meetod käivitatakse
        //saadab andmed serverisse, kus need salvestatakse andmebaasi

        [HttpPost]
        public async Task<IActionResult> Create(LibraryCreateUpdateViewModel vm)
        {
            //luua vaheinstants, mis sisaldab andmeid, mis on saadud vormist
            //need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamis

            var dto = new LibraryDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                BuildDate = vm.BuildDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                Files = vm.Files,
                FileToApiDtos = vm.Image
                    .Select(file => new FileToApiDto
                    {
                        Id = file.ImageId,
                        ExistingFilePath = /*"/multipleFileUpload/" +*/ file.FilePath,
                        LibraryId = file.LibraryId
                    }).ToArray()
            };

            //kutsuda teenuse meetodit, mis salvestab andmed andmebaasi
            var result = await _libraryService.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        //tuleb teha details meetod
        //see kutsub välja interfacest service meetodi

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {

            var library = await _libraryService.DetailsAsync(id);

            //veakäsitlus
            //suunab vatele NotFound, kui andmed ei ole
            if (library == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.LibraryId == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    LibraryId = y.LibraryId
                }).ToArrayAsync();

            //tuleb teha viewmodel ja see siin välja kutsuda
            //ära map'ida vm ja doamin

            var vm = new LibraryDetailsViewModel();

            vm.Id = library.Id;
            vm.Name = library.Name;
            vm.Classification = library.Classification;
            vm.BuildDate = library.BuildDate;
            vm.EnginePower = library.EnginePower;
            vm.Crew = library.Crew;
            vm.CreatedAt = library.CreatedAt;
            vm.ModifiedAt = library.ModifiedAt;
            vm.Images.AddRange(images);

            return View(vm);
        }
        //UPDATE
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {

            var library = await _libraryService.DetailsAsync(id);

            //veakäsitlus
            //suunab vatele NotFound, kui andmed ei ole
            if (library == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.LibraryId == id)
                   .Select(y => new ImageViewModel
                   {
                       FilePath = y.ExistingFilePath,
                       ImageId = y.Id,
                       LibraryId = y.LibraryId
                   }).ToArrayAsync();

            //tuleb teha viewmodel ja see siin välja kutsuda
            //ära map'ida vm ja doamin

            var vm = new LibraryCreateUpdateViewModel();

            vm.Id = library.Id;
            vm.Name = library.Name;
            vm.Classification = library.Classification;
            vm.BuildDate = library.BuildDate;
            vm.EnginePower = library.EnginePower;
            vm.Crew = library.Crew;
            vm.CreatedAt = library.CreatedAt;
            vm.ModifiedAt = library.ModifiedAt;
            vm.Image.AddRange(images);

            return View("CreateUpdate", vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(LibraryUpdateViewModel vm)
        {
            var dto = new LibraryDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                BuildDate = vm.BuildDate,
                EnginePower = vm.EnginePower,
                Crew = vm.Crew,
                Files = vm.Files,
                FileToApiDtos = vm.Images
                    .Select(file => new FileToApiDto
                    {
                        Id = file.ImageId,
                        ExistingFilePath = file.FilePath,
                        LibraryId = file.LibraryId
                    }).ToArray()
            };
            var result = await _libraryService.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));

        }

        //DELETE
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var library = await _libraryService.DetailsAsync(id);

            if (library == null)
            {
                return NotFound();
            }
            var images = await _context.FileToApis
                .Where(x => x.LibraryId == id)
                 .Select(y => new ImageViewModel
                 {
                     FilePath = y.ExistingFilePath,
                     ImageId = y.Id,
                     LibraryId = y.LibraryId
                 }).ToArrayAsync();

            var vm = new LibraryDeleteViewModel();

            vm.Id = library.Id;
            vm.Name = library.Name;
            vm.Classification = library.Classification;
            vm.BuildDate = library.BuildDate;
            vm.EnginePower = library.EnginePower;
            vm.Crew = library.Crew;
            vm.CreatedAt = library.CreatedAt;
            vm.ModifiedAt = library.ModifiedAt;

            vm.Images.AddRange(images);
            return View(vm);
        }
    }
}
