using ONEE_Stage.Models.Enums;

namespace ONEE_Stage.Models.DTO
{
    public class ApplicationDTO
    {
        public int Id { get; set; }
        public ApplicationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public int StudentId { get; set; }
        public string StudentFullName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string? StudentPhone { get; set; } 
        public string University { get; set; } = string.Empty;

        public int OfferId { get; set; }
        public string OfferTitle { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;

        public int? CvDocumentId { get; set; }
        public int? CoverLetterDocumentId { get; set; }
    }

    public class CreateApplicationDTO
    {
        public int StudentId { get; set; }
        public int OfferId { get; set; }
        public int? CvDocumentId { get; set; }
        public int? CoverLetterDocumentId { get; set; }
    }
}