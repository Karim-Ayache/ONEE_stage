using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONEE_Stage.Data;
using ONEE_Stage.Models.DTO;
using ONEE_Stage.Models.Entities;
using ONEE_Stage.Models.Enums;
using ONEE_Stage.Models.ViewModels;
using ONEE_Stage.Services;
using System.Security.Claims;

namespace ONEE_Stage.Controllers
{
    [Authorize]
    public class ApplicationsController : Controller
    {
        private readonly IApplicationsService _applicationsService;
        private readonly IOffersService _offersService;
        private readonly IDocumentService _fileStorageService;
        private readonly ApplicationDbContext _context;

        public ApplicationsController(
            IApplicationsService applicationsService,
            IOffersService offersService,
            IDocumentService fileStorageService,
            ApplicationDbContext context)
        {
            _applicationsService = applicationsService;
            _offersService = offersService;
            _fileStorageService = fileStorageService;
            _context = context;
        }

        // GET: Applications/Index?offerId=5 (HR/Staff application review for a specific offer)
        [Authorize(Roles = "HRManager,Admin,Supervisor")]
        [HttpGet]
        public async Task<IActionResult> Index(int offerId)
        {
            var offer = await _offersService.GetOfferByIdAsync(offerId);
            if (offer == null) return NotFound();

            var dtos = await _applicationsService.GetApplicationsByOfferAsync(offerId);

            var viewModels = dtos.Select(a => new ApplicationViewModel
            {
                Id = a.Id,
                Status = a.Status,
                CreatedAt = a.CreatedAt,
                StudentId = a.StudentId,
                StudentFullName = a.StudentFullName,
                StudentEmail = a.StudentEmail,
                StudentPhone = a.StudentPhone, // Fixed property name
                University = a.University,
                OfferId = a.OfferId,
                OfferTitle = a.OfferTitle,
                Department = a.Department,
                CvDocumentId = a.CvDocumentId,
                CoverLetterDocumentId = a.CoverLetterDocumentId
            }).ToList();

            ViewBag.Offer = offer;
            ViewBag.TotalCandidates = viewModels.Count;

            return View(viewModels);
        }

        // GET: Applications/Details/5
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _applicationsService.GetApplicationByIdAsync(id);
            if (dto == null) return NotFound();

            var viewModel = new ApplicationViewModel
            {
                Id = dto.Id,
                Status = dto.Status,
                CreatedAt = dto.CreatedAt,
                StudentId = dto.StudentId,
                StudentFullName = dto.StudentFullName,
                StudentEmail = dto.StudentEmail,
                StudentPhone = dto.StudentPhone, 
                University = dto.University,
                OfferId = dto.OfferId,
                OfferTitle = dto.OfferTitle,
                Department = dto.Department,
                CvDocumentId = dto.CvDocumentId,
                CoverLetterDocumentId = dto.CoverLetterDocumentId
            };

            return View(viewModel);
        }

        // POST: Applications/UpdateStatus
        [Authorize(Roles = "HRManager,Admin,Supervisor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, ApplicationStatus status)
        {
            var updated = await _applicationsService.UpdateStatusAsync(id, status);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Applications/Apply/5
        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IActionResult> Apply(int offerId)
        {
            var offer = await _offersService.GetOfferByIdAsync(offerId);
            if (offer == null) return NotFound();

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int studentId)) return Unauthorized();

            if (await _applicationsService.HasAlreadyAppliedAsync(studentId, offerId))
            {
                TempData["Error"] = "Vous avez déjà postulé à cette offre.";
                return RedirectToAction("Details", "Offers", new { id = offerId });
            }

            // Check if student already has profile documents on file
            var defaultCv = await _context.Documents
                .FirstOrDefaultAsync(d => d.StudentId == studentId && d.Type == DocumentType.CV && d.ApplicationId == null);

            var defaultCoverLetter = await _context.Documents
                .FirstOrDefaultAsync(d => d.StudentId == studentId && d.Type == DocumentType.CoverLetter && d.ApplicationId == null);

            var viewModel = new ApplicationViewModel
            {
                OfferId = offer.Id,
                OfferTitle = offer.Title,
                Department = offer.Department,
                CvDocumentId = defaultCv?.Id,
                CoverLetterDocumentId = defaultCoverLetter?.Id
            };

            return View(viewModel);
        }

        // POST: Applications/Apply
        [Authorize(Roles = "Student")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(int offerId, IFormFile? cvFile, IFormFile? coverLetterFile)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int currentStudentId)) return Unauthorized();

            // Fetch existing profile defaults
            var existingCv = await _context.Documents
                .FirstOrDefaultAsync(d => d.StudentId == currentStudentId && d.Type == DocumentType.CV && d.ApplicationId == null);

            var existingCoverLetter = await _context.Documents
                .FirstOrDefaultAsync(d => d.StudentId == currentStudentId && d.Type == DocumentType.CoverLetter && d.ApplicationId == null);

            int? cvDocId = existingCv?.Id;
            int? clDocId = existingCoverLetter?.Id;

            // Handle CV File (Use uploaded file OR existing profile default)
            if (cvFile != null && cvFile.Length > 0)
            {
                var newCv = await _fileStorageService.UploadDocumentAsync(cvFile, DocumentType.CV, currentStudentId);
                cvDocId = newCv.Id;
            }
            else if (existingCv == null)
            {
                ModelState.AddModelError("cvFile", "Le dépôt d'un CV est obligatoire.");
                var offer = await _offersService.GetOfferByIdAsync(offerId);
                return View(new ApplicationViewModel
                {
                    OfferId = offerId,
                    OfferTitle = offer?.Title ?? string.Empty,
                    Department = offer?.Department ?? string.Empty
                });
            }

            // Handle Cover Letter File
            if (coverLetterFile != null && coverLetterFile.Length > 0)
            {
                var newCl = await _fileStorageService.UploadDocumentAsync(coverLetterFile, DocumentType.CoverLetter, currentStudentId);
                clDocId = newCl.Id;
            }

            var dto = new CreateApplicationDTO
            {
                StudentId = currentStudentId,
                OfferId = offerId,
                CvDocumentId = cvDocId,
                CoverLetterDocumentId = clDocId
            };

            var created = await _applicationsService.CreateApplicationAsync(dto);
            if (!created)
            {
                TempData["Error"] = "Vous avez déjà postulé à cette offre.";
                return RedirectToAction("Details", "Offers", new { id = offerId });
            }

            TempData["SuccessMessage"] = "Votre candidature a été transmise avec succès.";
            return RedirectToAction(nameof(MyApplications));
        }

        // GET: Applications/MyApplications
        [Authorize(Roles = "Student")]
        [HttpGet]
        public async Task<IActionResult> MyApplications()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out int currentStudentId))
            {
                return Unauthorized();
            }

            var applications = await _applicationsService.GetApplicationsByStudentAsync(currentStudentId);

            var viewModels = applications.Select(a => new MyApplicationViewModel
            {
                ApplicationId = a.Id,
                OfferId = a.OfferId,
                OfferTitle = a.OfferTitle,
                Department = a.Department,
                AppliedAt = a.CreatedAt,
                Status = a.Status.ToString()
            }).ToList();

            return View(viewModels);
        }
    }
}