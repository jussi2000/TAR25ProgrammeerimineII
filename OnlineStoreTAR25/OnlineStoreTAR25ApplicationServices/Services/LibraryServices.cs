
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;

namespace OnlineStoreTAR25ApplicationServices.Services
{
    public class LibraryServices : ILibraryServices
    {
        private readonly LibraryContext _context;
        private readonly IFileServices _fileServices;

        public LibraryServices(LibraryContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<Book> AddBookAsync(LibraryDto dto)
        {
            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Author = dto.Author,
                PublishedYear = dto.PublishedYear,
                IsAvailable = true,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now
            };

            //salvestab failid wwwroot/multipleFileUpload kausta ja seob need raamatuga
            _fileServices.FilesToApi(dto, book);

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<IEnumerable<Book>> GetBooksByYearAsync(int year)
        {
            return await _context.Books
                .Where(b => b.PublishedYear == year)
                .ToListAsync();
        }

        public async Task<Book> GetBookByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return null;

            return await _context.Books
                .FirstOrDefaultAsync(
                    b => b.Title.ToLower().Contains(title.ToLower()));
        }

        public async Task<IEnumerable<Book>> GetCurrentlyBorrowedBooksAsync()
        {
            return await _context.Books
                .Where(b => b.IsAvailable == false)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetLoansByReaderAsync(int readerId)
        {
            return await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Reader)          // <-- uus rida
                .Where(l => l.ReaderId == readerId)
                .ToListAsync();
        }

        public async Task<Book> UpdateBookTitleAsync(Guid bookId, string newTitle)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
                throw new Exception("Raamatut ei leitud!");

            book.Title = newTitle;

            _context.Books.Update(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<bool> ReturnBookAsync(int loanId)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == loanId);

            if (loan == null || loan.ReturnDate != null)
                return false;

            loan.ReturnDate = DateTime.Now;
            loan.Book.IsAvailable = true;

            _context.Loans.Update(loan);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteBookAsync(Guid bookId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
                return false;

            var hasLoans = await _context.Loans
                .AnyAsync(l => l.BookId == bookId);

            if (hasLoans)
            {
                throw new InvalidOperationException(
                    "Raamatut ei saa kustutada, kuna see on seotud laenutustega.");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
