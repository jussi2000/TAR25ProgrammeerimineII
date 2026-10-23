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

        public async Task<IActionResult> Index(string? title, int? year, bool borrowed = false)
        {
            IEnumerable<Book> books;

            if (borrowed)
            {
                books = await _libraryService.GetCurrentlyBorrowedBooksAsync();
            }
            else if (year.HasValue)
            {
                books = await _libraryService.GetBooksByYearAsync(year.Value);
            }
            else if (!string.IsNullOrWhiteSpace(title))
            {
                var book = await _libraryService.GetBookByTitleAsync(title);
                books = book == null ? new List<Book>() : new List<Book> { book };
            }
            else
            {
                books = await _context.Books.ToListAsync();
            }

            var result = books
                .Select(x => new LibraryIndexViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author,
                    PublishedYear = x.PublishedYear,
                    IsAvailable = x.IsAvailable
                }).ToList();

            ViewBag.SearchTitle = title;
            ViewBag.Year = year;
            ViewBag.Borrowed = borrowed;
            ViewBag.Readers = await _libraryService.GetReadersAsync();

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
                IsAvailable = book.IsAvailable,
                CreatedAt = book.CreatedAt,
                ModifiedAt = book.ModifiedAt
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

            //uued failid, mis update vormis lisati
            var dto = new LibraryDto
            {
                Id = book.Id,
                Files = vm.Files
            };
            _fileService.FilesToApi(dto, book);

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
                IsAvailable = book.IsAvailable,
                CreatedAt = book.CreatedAt,
                ModifiedAt = book.ModifiedAt
            };

            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var images = await _context.FileToApis
                .Where(x => x.BookId == id)
                .Select(y => new FileToApiDto
                {
                    Id = y.Id,
                    ExistingFilePath = y.ExistingFilePath,
                    LibraryId = y.BookId
                }).ToArrayAsync();

            try
            {
                await _libraryService.DeleteBookAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Delete), new { id });
            }

            //kustutame ka raamatu failid
            await _fileService.RemoveImagesFromApi(images);

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTitle(Guid id, string newTitle)
        {
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                await _libraryService.UpdateBookTitleAsync(id, newTitle);
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        //raamatu laenutamine
        [HttpPost]
        public async Task<IActionResult> LoanBook(Guid bookId, int readerId)
        {
            try
            {
                await _libraryService.LoanBookAsync(bookId, readerId);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        //lugejate nimekiri ja lugeja lisamise vorm
        [HttpGet]
        public async Task<IActionResult> Readers()
        {
            var readers = await _libraryService.GetReadersAsync();
            return View(readers);
        }

        //lugeja lisamine
        [HttpPost]
        public async Task<IActionResult> AddReader(string fullName, string email)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Nimi ja e-post on kohustuslikud.";
                return RedirectToAction(nameof(Readers));
            }

            await _libraryService.AddReaderAsync(fullName, email);

            return RedirectToAction(nameof(Readers));
        }

        //ühe konkreetse lugeja laenutused
        [HttpGet]
        public async Task<IActionResult> ReaderLoans(int id)
        {
            var reader = await _context.Readers.FirstOrDefaultAsync(r => r.Id == id);

            if (reader == null)
            {
                return NotFound();
            }

            ViewBag.ReaderName = reader.FullName;
            ViewBag.ReaderId = reader.Id;

            var loans = await _libraryService.GetLoansByReaderAsync(id);
            return View(loans);
        }

        // raamatu tagastamine
        [HttpPost]
        public async Task<IActionResult> ReturnBook(int loanId, int readerId)
        {
            await _libraryService.ReturnBookAsync(loanId);

            return RedirectToAction(nameof(ReaderLoans), new { id = readerId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(imageViewModel vm)
        {
            var dto = new FileToApiDto()
            {
                Id = vm.ImageId
            };
            var image = await _fileService.RemoveImageFromApi(dto);

            if (image?.BookId != null)
            {
                return RedirectToAction(nameof(Update), new { id = image.BookId });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}