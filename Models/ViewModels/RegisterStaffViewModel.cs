using System.ComponentModel.DataAnnotations;

namespace ONEE_Stage.Models.ViewModels
{
    public class RegisterHRManagerViewModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro de téléphone est obligatoire.")]
        [Phone(ErrorMessage = "Format de numéro invalide.")]
        [Display(Name = "Numéro de téléphone")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"(?i)^[a-zA-Z0-9._%+-]+@onee\.ma$",
            ErrorMessage = "HR accounts must use an official ONEE email address (@onee.ma).")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterSupervisorViewModel
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le numéro de téléphone est obligatoire.")]
        [Phone(ErrorMessage = "Format de numéro invalide.")]
        [Display(Name = "Numéro de téléphone")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [RegularExpression(@"(?i)^[a-zA-Z0-9._%+-]+@onee\.ma$",
            ErrorMessage = "Supervisor accounts must use an official ONEE email address (@onee.ma).")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department/Direction is required.")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Specialization / Pôle is required.")]
        public string Specialization { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}