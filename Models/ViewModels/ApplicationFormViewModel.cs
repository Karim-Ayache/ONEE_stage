using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ONEE_Stage.Models.ViewModels
{
    public class ApplicationFormViewModel
    {
        public int OfferId { get; set; }
        public string OfferTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Veuillez télécharger votre CV.")]
        public IFormFile CvFile { get; set; } = null!;

        [Required(ErrorMessage = "Veuillez télécharger votre lettre de motivation.")]
        public IFormFile CoverLetterFile { get; set; } = null!;

        [StringLength(500)]
        public string AdditionalNotes { get; set; } = string.Empty;
    }
}