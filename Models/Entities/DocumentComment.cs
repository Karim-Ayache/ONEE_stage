namespace ONEE_Stage.Models.Entities
{
    public class DocumentComment
    {
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int DocumentId { get; set; }
        public Document Document { get; set; } = null!;

        public int AuthorUserId { get; set; }
        public User AuthorUser { get; set; } = null!;
    }
}
