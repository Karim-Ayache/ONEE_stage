using ONEE_Stage.Models.DTO;

namespace ONEE_Stage.Services
{
    public interface IOffersService
    {
        Task<IEnumerable<OfferDTO>> GetAllOffersAsync();
        Task<OfferDTO?> GetOfferByIdAsync(int id);
        Task CreateOfferAsync(OfferDTO offerDto);
        Task UpdateOfferAsync(OfferDTO offerDto);
        Task DeleteOfferAsync(int id);
    }
}