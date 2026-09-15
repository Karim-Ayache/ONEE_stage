using ONEE_Stage.Models.DTO;
using ONEE_Stage.Models.Enums;

namespace ONEE_Stage.Services
{
    public interface IApplicationsService
    {
        Task<IEnumerable<ApplicationDTO>> GetApplicationsByStudentAsync(int studentId);
        Task<IEnumerable<ApplicationDTO>> GetApplicationsByOfferAsync(int offerId);
        Task<ApplicationDTO?> GetApplicationByIdAsync(int id);
        Task<bool> CreateApplicationAsync(CreateApplicationDTO dto);
        Task<bool> UpdateStatusAsync(int applicationId, ApplicationStatus newStatus);
        Task<bool> HasAlreadyAppliedAsync(int studentId, int offerId);
    }
}