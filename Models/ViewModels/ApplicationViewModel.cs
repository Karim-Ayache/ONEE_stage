using System.ComponentModel.DataAnnotations;
using ONEE_Stage.Models.Enums;

namespace ONEE_Stage.Models.ViewModels
{
    public class ApplicationViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Statut de la candidature")]
        public ApplicationStatus Status { get; set; }

        [Display(Name = "Date de création")]
        public DateTime CreatedAt { get; set; }

        public int StudentId { get; set; }

        [Display(Name = "Candidat")]
        public string StudentFullName { get; set; } = string.Empty;

        [Display(Name = "Email institutionnel")]
        public string StudentEmail { get; set; } = string.Empty;

        [Display(Name = "Téléphone de contact")]
        public string? StudentPhone { get; set; }

        [Display(Name = "Université / Établissement")]
        public string University { get; set; } = string.Empty;

        public int OfferId { get; set; }

        [Display(Name = "Offre de stage")]
        public string OfferTitle { get; set; } = string.Empty;

        [Display(Name = "Département")]
        public string Department { get; set; } = string.Empty;

        [Display(Name = "Document CV")]
        public int? CvDocumentId { get; set; }

        [Display(Name = "Lettre de motivation")]
        public int? CoverLetterDocumentId { get; set; }
    }
}