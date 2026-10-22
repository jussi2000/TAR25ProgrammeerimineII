using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;
using ShopTARpe25.Models.Library;

namespace ShopTAR25.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILibraryServices _libraryService;
        private readonly LibraryContext _context;
        private readonly IFileServices _fileService;

        public LibraryController(
            ILibraryServices libraryService,
            LibraryContext context,
            IFileServices fileService)
        {
            _libraryService = libraryService;
            _context = context;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            var result = _context.Books
                .Select(x => new LibraryIndexViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    PublishedYear = x.PublishedYear,
                    IsAvailable = x.IsAvailable
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            LibraryCreateUpdateViewModel vm = new();
            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(LibraryCreateUpdateViewModel vm)
        {
            var dto = new LibraryDto
            {
                Id = vm.Id,
                Title = vm.Title,
                Author = vm.Author,
                PublishedYear = vm.PublishedYear,
                IsAvailable = vm.IsAvailable,
                Files = vm.Files,
                FileToApiDtos = vm.Images?
                    .Select(file => new FileToApiDto
                    {
                        Id = file.ImageId,
                        ExistingFilePath = file.FilePath,
                    }).ToArray()
            };

            await _libraryService.AddBookAsync(dto);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.BookId == id)
                .Select(y => new imageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    BookId = y.BookId
                }).ToArrayAsync();

            var vm = new LibraryDetailsViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedYear = book.PublishedYear,
                IsAvailable = book.IsAvailable,
                CreatedAt = book.CreatedAt,
                ModifiedAt = book.ModifiedAt
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.BookId == id)
                .Select(y => new imageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    BookId = y.BookId
                }).ToArrayAsync();

            var vm = new LibraryCreateUpdateViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedYear = book.PublishedYear,
                IsAvailable = book.IsAvailable
            };

            vm.Images.AddRange(images);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(LibraryCreateUpdateViewModel vm)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == vm.Id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = vm.Title;
            book.Author = vm.Author;
            book.PublishedYear = vm.PublishedYear;
            book.IsAvailable = vm.IsAvailable;
            book.ModifiedAt = DateTime.Now;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var images = await _context.FileToApis
                .Where(x => x.BookId == id)
                .Select(y => new imageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageId = y.Id,
                    BookId = y.BookId
                }).ToArrayAsync();

            var vm = new LibraryDeleteViewModel
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                PublishedYear = book.PublishedYear,
                IsAvailable = book.IsAvailable
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(imageViewModel vm)
        {
            var dto = new FileToApiDto()
            {
                Id = vm.ImageId
            };
            var image = await _fileService.RemoveImageFromApi(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}