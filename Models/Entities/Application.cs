using ONEE_Stage.Models.Enums;

namespace ONEE_Stage.Models.Entities
{
    public class Application
    {
        public int Id { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Draft;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int OfferId { get; set; }
        public Offer Offer { get; set; } = null!;

        // Isolated Document References per Application
        public int? CvDocumentId { get; set; }
        public Document? CvDocument { get; set; }

        public int? CoverLetterDocumentId { get; set; }
        public Document? CoverLetterDocument { get; set; }

        // Navigation
        public AIScore? AIScore { get; set; }
        public ICollection<DocumentComment> Comments { get; set; } = new List<DocumentComment>();
    }
}