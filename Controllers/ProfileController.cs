using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ONEE_Stage.Models.ViewModels;
using System.Security.Claims;

namespace ONEE_Stage.Controllers
{
    [Authorize(Roles = "Student")]
    public class ProfileController : Controller
    {
        // Inject your application DbContext or ProfileService here

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Fetch current student profile from your database using User.FindFirstValue(ClaimTypes.NameIdentifier)
            var model = new StudentProfileViewModel
            {
                FullName = User.Identity?.Name ?? "Étudiant ONEE",
                Email = User.FindFirstValue(ClaimTypes.Email) ?? "",
                Phone = "+212 6 00 00 00 00",
                University = "EMSI Casablanca",
                Specialization = "Génie Informatique & Réseaux",
                CvDocumentId = 1,
                CvFileName = "CV_2026_Ingenieur.pdf",
                CoverLetterDocumentId = null
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(StudentProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 1. Update text fields (FullName, Phone, University, Specialization) in Database

            // 2. Process CV upload if provided
            if (model.CvFile != null && model.CvFile.Length > 0)
            {
                // Save CV to file system/database and update model.CvDocumentId
            }

            // 3. Process Cover Letter upload if provided
            if (model.CoverLetterFile != null && model.CoverLetterFile.Length > 0)
            {
                // Save Cover Letter to file system/database and update model.CoverLetterDocumentId
            }

            TempData["SuccessMessage"] = "Votre profil a été mis à jour avec succès !";
            return RedirectToAction(nameof(Index));
        }
    }
}