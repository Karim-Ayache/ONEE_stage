namespace ONEE_Stage.Models.ViewModels
{
    public class OfferDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Periode { get; set; } = string.Empty;
        public int AvailablePositions { get; set; }
        public DateTime Deadline { get; set; }
    }
}