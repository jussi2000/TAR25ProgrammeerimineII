using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.Serviceinterface
{
    public interface ILibraryServices
    {
        Task<Book> Create(Book dto);

        Task<Book> DetailsAsync(int id);

        Task<Book> Update(Book dto);

        Task<Book> Delete(int id);

    }
}
