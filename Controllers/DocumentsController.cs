using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONEE_Stage.Services;

namespace ONEE_Stage.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        // GET: Documents/View/5 (Inline PDF Streaming)
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);

            if (document == null || document.FileData == null || document.FileData.Length == 0)
            {
                return NotFound("The requested document was not found.");
            }

            Response.Headers.Append("Content-Disposition", $"inline; filename=\"{document.FileName}\"");
            return File(document.FileData, document.ContentType ?? "application/pdf");
        }

        // GET: Documents/Download/5 (Direct File Attachment Download)
        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            var document = await _documentService.GetDocumentByIdAsync(id);

            if (document == null || document.FileData == null || document.FileData.Length == 0)
            {
                return NotFound("The requested document was not found.");
            }

            return File(document.FileData, document.ContentType ?? "application/pdf", document.FileName);
        }
    }
}