using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ONEE_Stage.Data;
using ONEE_Stage.Models.Entities;
using ONEE_Stage.Models.Enums;

namespace ONEE_Stage.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _context;
        private const long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB Limit

        public DocumentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Document> UploadDocumentAsync(IFormFile file, DocumentType type, int studentId, int? applicationId = null)
        {
            // Validation: Must exist
            if (file == null || file.Length == 0)
                throw new ArgumentException("The file is empty or invalid.");

            // Validation: Must be PDF
            if (file.ContentType != "application/pdf" && !file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Only PDF files are allowed.");

            // Validation: Size limit
            if (file.Length > MaxFileSizeInBytes)
                throw new InvalidOperationException("The file size exceeds the allowed limit (5 MB).");

            // Convert to byte array for DB storage
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var document = new Document
            {
                FileName = Path.GetFileName(file.FileName),
                ContentType = "application/pdf",
                Type = type,
                FileData = memoryStream.ToArray(),
                UploadedAt = DateTime.UtcNow,
                StudentId = studentId,
                ApplicationId = applicationId,
                IsUnlockedForEdit = false
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return document;
        }

        public async Task<Document?> GetDocumentByIdAsync(int documentId)
        {
            return await _context.Documents
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == documentId);
        }

        public async Task<bool> DeleteDocumentAsync(int documentId)
        {
            var document = await _context.Documents.FindAsync(documentId);
            if (document == null) return false;

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UnlockDocumentForEditAsync(int documentId)
        {
            var doc = await _context.Documents.FindAsync(documentId);
            if (doc != null)
            {
                doc.IsUnlockedForEdit = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}