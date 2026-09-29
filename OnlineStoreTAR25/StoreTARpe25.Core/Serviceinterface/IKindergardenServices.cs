using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.Serviceinterface
{
    public interface IKindergardenServices
    {
        Task<Kindergarden> Create(KindergardenDto dto);

        Task<Kindergarden> DetailsAsync(Guid id);

        Task<Kindergarden> Update(KindergardenDto id);

        Task<Kindergarden> Delete(Guid id);

    }
}
