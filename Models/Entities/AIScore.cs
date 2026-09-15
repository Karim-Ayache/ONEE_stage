namespace ONEE_Stage.Models.Entities
{
    public class AIScore
    {
        public int Id { get; set; }

        // Matching score percentage (e.g., 85.5)
        public double OverallScore { get; set; }
        public string Summary { get; set; } = string.Empty;
        public DateTime EvaluatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key & Navigation to Application
        public int ApplicationId { get; set; }
        public Application Application { get; set; } = null!;
    }
}