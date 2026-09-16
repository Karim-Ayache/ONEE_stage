using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONEE_Stage.Data;
using ONEE_Stage.Models.Entities;
using ONEE_Stage.Models.Enums;
using System.Security.Claims;

namespace ONEE_Stage.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }
            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == userId);
            if (student == null) return NotFound("Profil étudiant introuvable.");

            // Charger les documents de l'étudiant pour affichage / actions (aperçu, téléchargement, suppression)
            var documents = await _context.Documents
                .Where(d => d.StudentId == userId)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();

            ViewBag.StudentDocuments = documents;

            return View(student);
        }

        [HttpGet]
        public async Task<IActionResult> ViewDocument(int id)
        {
            var doc = await _context.Documents.FindAsync(id);
            if (doc == null) return NotFound();

            // Vérifier que l'utilisateur actuel est propriétaire du document
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId) || doc.StudentId != userId)
                return Forbid();

            // Renvoyer le fichier pour affichage inline (le navigateur décidera du rendu)
            return File(doc.FileData, doc.ContentType);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadDocument(int id)
        {
            var doc = await _context.Documents.FindAsync(id);
            if (doc == null) return NotFound();

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId) || doc.StudentId != userId)
                return Forbid();

            // Forcer le téléchargement avec le nom de fichier
            return File(doc.FileData, doc.ContentType, doc.FileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var doc = await _context.Documents.FindAsync(id);
            if (doc == null) return NotFound();

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId) || doc.StudentId != userId)
                return Forbid();

            _context.Documents.Remove(doc);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Document supprimé.";
            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(Student model, string phone, IFormFile? cvFile, IFormFile? coverLetterFile)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var student = await _context.Students.FindAsync(userId);
            if (student == null) return NotFound();

            // Update basic text fields
            student.University = model.University;
            student.InstitutionalEmail = model.InstitutionalEmail;
            // N'assigner Phone que si une valeur non vide est fournie pour éviter d'écraser
            // une valeur existante par NULL lors d'un SaveChanges.
            if (!string.IsNullOrWhiteSpace(phone))
            {
                student.Phone = phone;
            }

            // 1. Process CV Upload
            if (cvFile != null && cvFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await cvFile.CopyToAsync(memoryStream);

                var cvDoc = new Document
                {
                    FileName = cvFile.FileName,
                    ContentType = cvFile.ContentType,
                    FileData = memoryStream.ToArray(),
                    Type = DocumentType.CV, // Assuming this exists in your enum
                    StudentId = userId,
                    UploadedAt = DateTime.UtcNow
                };

                _context.Documents.Add(cvDoc);
                await _context.SaveChangesAsync(); // Save to generate the Document ID

                student.DefaultCvDocumentId = cvDoc.Id;
            }

            // 2. Process Cover Letter Upload
            if (coverLetterFile != null && coverLetterFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await coverLetterFile.CopyToAsync(memoryStream);

                var lmDoc = new Document
                {
                    FileName = coverLetterFile.FileName,
                    ContentType = coverLetterFile.ContentType,
                    FileData = memoryStream.ToArray(),
                    Type = DocumentType.CoverLetter, // Assuming this exists in your enum
                    StudentId = userId,
                    UploadedAt = DateTime.UtcNow
                };

                _context.Documents.Add(lmDoc);
                await _context.SaveChangesAsync(); // Save to generate the Document ID

                student.DefaultCoverLetterDocumentId = lmDoc.Id;
            }

            // Save the updated student profile with the new Default Document IDs
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Profil et documents mis à jour avec succès !";
            return RedirectToAction(nameof(Profile));
        }
    }
}