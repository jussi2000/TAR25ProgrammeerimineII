
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.Serviceinterface
{
    public interface ILibraryServices
    {
        Task<Book> AddBookAsync(LibraryDto dto);
        Task<IEnumerable<Book>> GetBooksByYearAsync(int year);
        Task<Book> GetBookByTitleAsync(string title);
        Task<IEnumerable<Book>> GetCurrentlyBorrowedBooksAsync();
        Task<IEnumerable<Loan>> GetLoansByReaderAsync(int readerId);
        Task<Book> UpdateBookTitleAsync(Guid bookId, string newTitle);
        Task<bool> ReturnBookAsync(int loanId);
        Task<bool> DeleteBookAsync(Guid bookId);
    }
}
