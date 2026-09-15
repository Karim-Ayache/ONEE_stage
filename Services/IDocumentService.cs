using Microsoft.AspNetCore.Http;
using ONEE_Stage.Models.Entities;
using ONEE_Stage.Models.Enums;
using System.Threading.Tasks;

namespace ONEE_Stage.Services
{
    public interface IDocumentService
    {
        Task<Document> UploadDocumentAsync(IFormFile file, DocumentType type, int studentId, int? applicationId = null);
        Task<Document?> GetDocumentByIdAsync(int documentId);
        Task<bool> DeleteDocumentAsync(int documentId);
        Task UnlockDocumentForEditAsync(int documentId);
    }
}