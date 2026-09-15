using ONEE_Stage.Models.Enums;

namespace ONEE_Stage.Models.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public DocumentType Type { get; set; }

        // Stored as VARBINARY(MAX) in SQL Server
        public byte[] FileData { get; set; } = Array.Empty<byte>();

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public bool IsUnlockedForEdit { get; set; } = false;

        // Foreign Keys & Navigations
        public int StudentId { get; set; }
        public Student? Student { get; set; }

        public int? ApplicationId { get; set; }
        public Application? Application { get; set; }
    }
}