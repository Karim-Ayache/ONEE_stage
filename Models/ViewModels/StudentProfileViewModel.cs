using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ONEE_Stage.Models.ViewModels
{
    public class StudentProfileViewModel
    {
        public string StudentId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom complet est obligatoire.")]
        [Display(Name = "Nom Complet")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'adresse email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        [Display(Name = "Adresse Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro de téléphone est obligatoire.")]
        [Phone(ErrorMessage = "Numéro de téléphone invalide.")]
        [Display(Name = "Numéro de Téléphone")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'établissement/université est obligatoire.")]
        [Display(Name = "Établissement / Université")]
        public string University { get; set; } = string.Empty;

        [Display(Name = "Spécialité / Filière")]
        public string Specialization { get; set; } = string.Empty;

        // Document tracking
        public int? CvDocumentId { get; set; }
        public string? CvFileName { get; set; }

        public int? CoverLetterDocumentId { get; set; }
        public string? CoverLetterFileName { get; set; }

        // Upload inputs
        [Display(Name = "Curriculum Vitae (PDF)")]
        public IFormFile? CvFile { get; set; }

        [Display(Name = "Lettre de Motivation (PDF)")]
        public IFormFile? CoverLetterFile { get; set; }
    }
}