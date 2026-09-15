namespace ONEE_Stage.Models.Entities
{
    public class Student : User
    {
        public string InstitutionalEmail { get; set; } = string.Empty;
        public bool IsOtpValidated { get; set; } = false;
        public string University { get; set; } = string.Empty;
        public DateTime LastActivityAt { get; set; } = DateTime.UtcNow;
        public bool IsAlumni { get; set; } = false;

        public int FiliereId { get; set; }
        public int? SpecialiteId { get; set; }
        public int? DefaultCvDocumentId { get; set; }
        public int? DefaultCoverLetterDocumentId { get; set; }
    }
}
