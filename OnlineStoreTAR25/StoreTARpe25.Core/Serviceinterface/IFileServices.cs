
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.Serviceinterface
{
    public interface IFileServices
    {
        void FilesToApi(KindergardenDto dto, Kindergarden domain);
        Task<FileToApi?> RemoveImageFromApi(FileToApiDto dto);
        Task<List<FileToApi>> RemoveImagesFromApi(FileToApiDto[] dto);
    }
}
