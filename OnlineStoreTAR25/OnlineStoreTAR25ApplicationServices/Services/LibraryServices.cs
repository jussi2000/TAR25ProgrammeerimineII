
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
                .Include(l => l.Reader)
                .Where(l => l.ReaderId == readerId)
                .ToListAsync();
        }

        public async Task<Book> UpdateBookTitleAsync(Guid bookId, string newTitle)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
                throw new Exception("Raamatut ei leitud!");

            book.Title = newTitle;
            book.ModifiedAt = DateTime.Now;

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

            var hasActiveLoans = await _context.Loans
                .AnyAsync(l => l.BookId == bookId && l.ReturnDate == null);

            if (hasActiveLoans)
            {
                throw new InvalidOperationException(
                    "Raamatut ei saa kustutada, kuna see on praegu laenutuses.");
            }

            var returnedLoans = _context.Loans.Where(l => l.BookId == bookId);
            _context.Loans.RemoveRange(returnedLoans);

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Reader> AddReaderAsync(string fullName, string email)
        {
            var reader = new Reader
            {
                FullName = fullName,
                Email = email
            };

            await _context.Readers.AddAsync(reader);
            await _context.SaveChangesAsync();

            return reader;
        }

        //lugejad, soovi korral otsing nime või e-posti järgi
        public async Task<IEnumerable<Reader>> GetReadersAsync(string? search = null)
        {
            var query = _context.Readers
                .Include(r => r.Loans)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    r.FullName.ToLower().Contains(search.ToLower()) ||
                    r.Email.ToLower().Contains(search.ToLower()));
            }

            return await query
                .OrderBy(r => r.FullName)
                .ToListAsync();
        }

        //lugeja kustutamine: ei saa kustutada, kui tal on tagastamata raamatuid
        public async Task<bool> DeleteReaderAsync(int readerId)
        {
            var reader = await _context.Readers.FindAsync(readerId);

            if (reader == null)
                return false;

            var hasActiveLoans = await _context.Loans
                .AnyAsync(l => l.ReaderId == readerId && l.ReturnDate == null);

            if (hasActiveLoans)
            {
                throw new InvalidOperationException(
                    "Lugejat ei saa kustutada, kuna tal on tagastamata raamatuid.");
            }

            //tagastatud laenutused kustutame koos lugejaga
            var returnedLoans = _context.Loans.Where(l => l.ReaderId == readerId);
            _context.Loans.RemoveRange(returnedLoans);

            _context.Readers.Remove(reader);
            await _context.SaveChangesAsync();

            return true;
        }

        //raamatu laenutamine: raamat peab olema saadaval, pärast laenutamist on saadavus false
        public async Task<Loan> LoanBookAsync(Guid bookId, int readerId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null)
                throw new InvalidOperationException("Raamatut ei leitud!");

            if (!book.IsAvailable)
                throw new InvalidOperationException("Raamat on juba välja laenutatud!");

            var readerExists = await _context.Readers.AnyAsync(r => r.Id == readerId);

            if (!readerExists)
                throw new InvalidOperationException("Lugejat ei leitud!");

            var loan = new Loan
            {
                BookId = bookId,
                ReaderId = readerId,
                LoanDate = DateTime.Now,
                ReturnDate = null
            };

            book.IsAvailable = false;
            book.ModifiedAt = DateTime.Now;

            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();

            return loan;
        }
    }
}
