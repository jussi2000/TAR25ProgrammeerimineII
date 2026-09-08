using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.Serviceinterface
{
    public interface IspaceshipServices
    {
        Task<Spaceship> Create(SpaceshipDto dto);

    }
}
