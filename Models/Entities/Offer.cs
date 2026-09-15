using ONEE_Stage.Models.Enums;

namespace ONEE_Stage.Models.Entities
{
    public class Offer
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;

        public InternshipType Type { get; set; }
        public WorkMode WorkMode { get; set; }
        public WorkPace WorkPace { get; set; }
        public OfferStatus Status { get; set; } = OfferStatus.Draft;

        public string? Periode { get; set; }
        public int AvailablePositions { get; set; }
        public DateTime Deadline { get; set; }

        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}