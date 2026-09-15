using System.ComponentModel.DataAnnotations;
using ONEE_Stage.Models.Enums;

namespace ONEE_Stage.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "L'adresse email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        [Display(Name = "Adresse Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire.")]
        [Display(Name = "Prénom")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [Display(Name = "Nom")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro de téléphone est obligatoire.")]
        [Phone(ErrorMessage = "Format de numéro invalide.")]
        [Display(Name = "Numéro de téléphone")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe")]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "Rôle demandé")]
        public UserRole Role { get; set; } = UserRole.Student;

        // Student-specific fields
        [Display(Name = "Université / École")]
        public string? University { get; set; }

        [Display(Name = "Email Institutionnel")]
        public string? InstitutionalEmail { get; set; }
    }
}