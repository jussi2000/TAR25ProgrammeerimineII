using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStoreTAR25ApplicationServices.Services;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;

namespace ShopTAR25.Controllers
{
    public class LibraryController : Controller
    {
        private readonly LibraryServices _libraryService;
        private readonly LibraryContext _context;
        private readonly IFileServices _fileService;

        public LibraryController(
            LibraryServices libraryService,
            LibraryContext context,
            IFileServices fileService)
        {
            _libraryService = libraryService;
            _context = context;
            _fileService = fileService;
        }

        // 1. Kuvab kõik raamatud või peamise vaate
        public async Task<IActionResult> Index()
        {
            var books = await _context.Books.ToListAsync();
            return View(books);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(LibraryDto dto)
        {
            dto.IsAvailable = true; 
            await _libraryService.AddBookAsync(dto);

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> GetByYear(int year)
        {
            var books = await _libraryService.GetBooksByYearAsync(year);
            return View("Index", books);
        }


        public async Task<IActionResult> GetByTitle(string title)
        {
            var book = await _libraryService.GetBookByTitleAsync(title);
            return View("Details", book);
        }


        public async Task<IActionResult> BorrowedBooks()
        {
            var books = await _libraryService.GetCurrentlyBorrowedBooksAsync();
            return View("Index", books);
        }


        public async Task<IActionResult> ReaderLoans(int readerId)
        {
            var loans = await _libraryService.GetLoansByReaderAsync(readerId);
            return View(loans);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateTitle(int id, string newTitle)
        {
            try
            {
                await _libraryService.UpdateBookTitleAsync(id, newTitle);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ReturnBook(int loanId)
        {
            await _libraryService.ReturnBookAsync(loanId);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _libraryService.DeleteBookAsync(id);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}