using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;
using System.Threading.Tasks;

namespace ShopTARpe25.Core.Serviceinterface
{
    public interface IFileServices
    {
        Task<FileToApi> UploadFilesToApi(FileToApiDto dto);
        Task<FileToApi> RemoveImage(FileToApiDto dto);
    }
}