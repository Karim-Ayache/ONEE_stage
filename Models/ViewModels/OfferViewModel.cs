using System;
using System.ComponentModel.DataAnnotations;
using ONEE_Stage.Models.Enums;

namespace ONEE_Stage.Models.ViewModels
{
    public class OfferViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The offer title is required.")]
        [StringLength(150, ErrorMessage = "The title cannot exceed 150 characters.")]
        [Display(Name = "Offer Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "The description is required.")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "The department is required.")]
        [Display(Name = "Department")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "The specialization is required.")]
        [Display(Name = "Specialization")]
        public string Specialization { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select the internship type.")]
        [Display(Name = "Internship Type")]
        public InternshipType Type { get; set; }

        [Required(ErrorMessage = "Please select the work mode.")]
        [Display(Name = "Work Mode")]
        public WorkMode WorkMode { get; set; }

        [Required(ErrorMessage = "Please select the work pace.")]
        [Display(Name = "Work Pace")]
        public WorkPace WorkPace { get; set; }

        [Display(Name = "Status")]
        public OfferStatus Status { get; set; } = OfferStatus.Draft;

        [Display(Name = "Period / Duration")]
        public string? Periode { get; set; }

        [Required(ErrorMessage = "Specify the number of available positions.")]
        [Range(1, 100, ErrorMessage = "The number of available positions must be at least 1.")]
        [Display(Name = "Available Positions")]
        public int AvailablePositions { get; set; }

        [Required(ErrorMessage = "The deadline date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Application Deadline")]
        public DateTime Deadline { get; set; } = DateTime.Today.AddDays(30);
    }
}