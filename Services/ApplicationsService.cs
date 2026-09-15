using Microsoft.EntityFrameworkCore;
using ONEE_Stage.Data;
using ONEE_Stage.Models.DTO;
using ONEE_Stage.Models.Entities;
using ONEE_Stage.Models.Enums;

namespace ONEE_Stage.Services
{
    public class ApplicationsService : IApplicationsService
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationDTO>> GetApplicationsByStudentAsync(int studentId)
        {
            return await _context.Applications
                .AsNoTracking()
                .Include(a => a.Offer)
                .Include(a => a.Student)
                .Where(a => a.StudentId == studentId)
                .Select(a => MapToDTO(a))
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationDTO>> GetApplicationsByOfferAsync(int offerId)
        {
            return await _context.Applications
                .AsNoTracking()
                .Include(a => a.Offer)
                .Include(a => a.Student)
                .Where(a => a.OfferId == offerId)
                .Select(a => MapToDTO(a))
                .ToListAsync();
        }

        public async Task<ApplicationDTO?> GetApplicationByIdAsync(int id)
        {
            var app = await _context.Applications
                .AsNoTracking()
                .Include(a => a.Offer)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);

            return app == null ? null : MapToDTO(app);
        }

        public async Task<bool> HasAlreadyAppliedAsync(int studentId, int offerId)
        {
            return await _context.Applications
                .AnyAsync(a => a.StudentId == studentId && a.OfferId == offerId);
        }

        public async Task<bool> CreateApplicationAsync(CreateApplicationDTO dto)
        {
            if (await HasAlreadyAppliedAsync(dto.StudentId, dto.OfferId))
                return false;

            var entity = new Application
            {
                StudentId = dto.StudentId,
                OfferId = dto.OfferId,
                CvDocumentId = dto.CvDocumentId,
                CoverLetterDocumentId = dto.CoverLetterDocumentId,
                Status = ApplicationStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            _context.Applications.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(int applicationId, ApplicationStatus newStatus)
        {
            var entity = await _context.Applications.FindAsync(applicationId);
            if (entity == null) return false;

            entity.Status = newStatus;
            _context.Applications.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        private static ApplicationDTO MapToDTO(Application a) => new()
        {
            Id = a.Id,
            Status = a.Status,
            CreatedAt = a.CreatedAt,
            StudentId = a.StudentId,
            StudentFullName = $"{a.Student?.FirstName} {a.Student?.LastName}".Trim(),
            StudentEmail = a.Student?.InstitutionalEmail ?? a.Student?.Email ?? string.Empty,
            StudentPhone = a.Student?.Phone,
            University = a.Student?.University ?? string.Empty,
            OfferId = a.OfferId,
            OfferTitle = a.Offer?.Title ?? string.Empty,
            Department = a.Offer?.Department ?? string.Empty,
            CvDocumentId = a.CvDocumentId,
            CoverLetterDocumentId = a.CoverLetterDocumentId
        };
    }
}