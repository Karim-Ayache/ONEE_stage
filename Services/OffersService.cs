using Microsoft.EntityFrameworkCore;
using ONEE_Stage.Data;
using ONEE_Stage.Models.Entities;
using ONEE_Stage.Models.DTO;
using ONEE_Stage.Models.Enums;

namespace ONEE_Stage.Services
{
    public class OffersService : IOffersService
    {
        private readonly ApplicationDbContext _context;

        public OffersService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OfferDTO>> GetAllOffersAsync()
        {
            return await _context.Offers
                .AsNoTracking()
                .Select(o => new OfferDTO
                {
                    Id = o.Id,
                    Title = o.Title,
                    Description = o.Description,
                    Department = o.Department,
                    Specialization = o.Specialization,
                    Type = o.Type,
                    WorkMode = o.WorkMode,
                    WorkPace = o.WorkPace,
                    Status = o.Status,
                    Periode = o.Periode,
                    AvailablePositions = o.AvailablePositions,
                    Deadline = o.Deadline
                })
                .ToListAsync();
        }

        public async Task<OfferDTO?> GetOfferByIdAsync(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null) return null;

            return new OfferDTO
            {
                Id = offer.Id,
                Title = offer.Title,
                Description = offer.Description,
                Department = offer.Department,
                Specialization = offer.Specialization,
                Type = offer.Type,
                WorkMode = offer.WorkMode,
                WorkPace = offer.WorkPace,
                Status = offer.Status,
                Periode = offer.Periode,
                AvailablePositions = offer.AvailablePositions,
                Deadline = offer.Deadline
            };
        }

        public async Task CreateOfferAsync(OfferDTO offerDto)
        {
            var offer = new Offer
            {
                Title = offerDto.Title,
                Description = offerDto.Description,
                Department = offerDto.Department,
                Specialization = offerDto.Specialization,
                Type = offerDto.Type,
                WorkMode = offerDto.WorkMode,
                WorkPace = offerDto.WorkPace,
                Status = OfferStatus.Draft,
                Periode = offerDto.Periode,
                AvailablePositions = offerDto.AvailablePositions,
                Deadline = offerDto.Deadline
            };

            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOfferAsync(OfferDTO offerDto)
        {
            var offer = await _context.Offers.FindAsync(offerDto.Id);
            if (offer == null) return;

            offer.Title = offerDto.Title;
            offer.Description = offerDto.Description;
            offer.Department = offerDto.Department;
            offer.Specialization = offerDto.Specialization;
            offer.Type = offerDto.Type;
            offer.WorkMode = offerDto.WorkMode;
            offer.WorkPace = offerDto.WorkPace;
            offer.Status = offerDto.Status;
            offer.Periode = offerDto.Periode;
            offer.AvailablePositions = offerDto.AvailablePositions;
            offer.Deadline = offerDto.Deadline;

            _context.Offers.Update(offer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOfferAsync(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer != null)
            {
                _context.Offers.Remove(offer);
                await _context.SaveChangesAsync();
            }
        }
    }
}