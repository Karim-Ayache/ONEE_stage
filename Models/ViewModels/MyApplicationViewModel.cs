using System;

namespace ONEE_Stage.Models.ViewModels
{
    public class MyApplicationViewModel
    {
        public int ApplicationId { get; set; }
        public int OfferId { get; set; }
        public string OfferTitle { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime AppliedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}