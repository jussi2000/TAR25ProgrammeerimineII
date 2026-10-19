using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.Serviceinterface
{
    public interface ILibraryServices
    {
        Task<Library> Create(LibraryDto dto);

        Task<Library> DetailsAsync(Guid id);

        Task<Library> Update(LibraryDto id);

        Task<Library> Delete(Guid id);

    }
}
