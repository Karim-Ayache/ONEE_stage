using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ONEE_Stage.Models.DTO;
using ONEE_Stage.Models.Enums;
using ONEE_Stage.Models.ViewModels;
using ONEE_Stage.Services;

namespace ONEE_Stage.Controllers
{
    public class OffersController : Controller
    {
        private readonly IOffersService _offersService;

        public OffersController(IOffersService offersService)
        {
            _offersService = offersService;
        }

        // GET: Offers (Public/Student catalog + HR Dashboard)
        public async Task<IActionResult> Index(string searchString, string department, OfferStatus? status)
        {
            var offerDtos = await _offersService.GetAllOffersAsync();

            // 1. ROLE-BASED VISIBILITY: Students ONLY see 'Published' offers
            bool isStaff = User.IsInRole("Admin") || User.IsInRole("HRManager");
            if (!isStaff)
            {
                // Force filter for students/public
                offerDtos = offerDtos.Where(o => o.Status == OfferStatus.Published).ToList();
            }

            // 2. APPLY SEARCH AND FILTERS
            if (!string.IsNullOrEmpty(searchString))
            {
                offerDtos = offerDtos.Where(o =>
                    (o.Title != null && o.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase)) ||
                    (o.Specialization != null && o.Specialization.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                ).ToList();
            }

            if (!string.IsNullOrEmpty(department))
            {
                offerDtos = offerDtos.Where(o => o.Department == department).ToList();
            }

            if (status.HasValue && isStaff)
            {
                offerDtos = offerDtos.Where(o => o.Status == status.Value).ToList();
            }

            // 3. MAP TO VIEW MODEL
            var offerViewModels = offerDtos.Select(o => new OfferViewModel
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                Department = o.Department,
                Specialization = o.Specialization,
                Type = o.Type,
                WorkMode = o.WorkMode,
                WorkPace = o.WorkPace,
                Status = o.Status,
                Periode = o.Periode,
                AvailablePositions = o.AvailablePositions,
                Deadline = o.Deadline
            }).OrderByDescending(o => o.Id).ToList(); // Newest first

            // 4. POPULATE DROPDOWNS FOR THE FILTER UI
            var allOffersForDropdowns = await _offersService.GetAllOffersAsync();
            var departments = allOffersForDropdowns
                                .Select(o => o.Department)
                                .Where(d => !string.IsNullOrEmpty(d))
                                .Distinct()
                                .OrderBy(d => d)
                                .ToList();

            ViewBag.Departments = new SelectList(departments, department);
            ViewBag.CurrentSearch = searchString;
            ViewBag.CurrentStatus = status;

            return View(offerViewModels);
        }

        // GET: Offers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var o = await _offersService.GetOfferByIdAsync(id);
            if (o == null) return NotFound();

            // Security check: If a student tries to direct-link to a Draft, block them
            bool isStaff = User.IsInRole("Admin") || User.IsInRole("HRManager");
            if (!isStaff && o.Status != OfferStatus.Published)
            {
                return Unauthorized();
            }

            var viewModel = new OfferViewModel
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                Department = o.Department,
                Specialization = o.Specialization,
                Type = o.Type,
                WorkMode = o.WorkMode,
                WorkPace = o.WorkPace,
                Status = o.Status,
                Periode = o.Periode,
                AvailablePositions = o.AvailablePositions,
                Deadline = o.Deadline
            };

            return View(viewModel);
        }

        // GET: Offers/Create
        [Authorize(Roles = "HRManager,Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Offers/Create
        [Authorize(Roles = "HRManager,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OfferViewModel viewModel)
        {
            if (!ModelState.IsValid) return View(viewModel);

            var offerDto = new OfferDTO
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                Department = viewModel.Department,
                Specialization = viewModel.Specialization,
                Type = viewModel.Type,
                WorkMode = viewModel.WorkMode,
                WorkPace = viewModel.WorkPace,
                Periode = viewModel.Periode,
                AvailablePositions = viewModel.AvailablePositions,
                Deadline = viewModel.Deadline,
                Status = OfferStatus.Draft // Default to draft on creation
            };

            await _offersService.CreateOfferAsync(offerDto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Offers/Edit/5
        [Authorize(Roles = "HRManager,Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var o = await _offersService.GetOfferByIdAsync(id);
            if (o == null) return NotFound();

            var viewModel = new OfferViewModel
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                Department = o.Department,
                Specialization = o.Specialization,
                Type = o.Type,
                WorkMode = o.WorkMode,
                WorkPace = o.WorkPace,
                Status = o.Status,
                Periode = o.Periode,
                AvailablePositions = o.AvailablePositions,
                Deadline = o.Deadline
            };

            return View(viewModel);
        }

        // POST: Offers/Edit/5
        [Authorize(Roles = "HRManager,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OfferViewModel viewModel)
        {
            if (id != viewModel.Id) return BadRequest();
            if (!ModelState.IsValid) return View(viewModel);

            var offerDto = new OfferDTO
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Description = viewModel.Description,
                Department = viewModel.Department,
                Specialization = viewModel.Specialization,
                Type = viewModel.Type,
                WorkMode = viewModel.WorkMode,
                WorkPace = viewModel.WorkPace,
                Status = viewModel.Status,
                Periode = viewModel.Periode,
                AvailablePositions = viewModel.AvailablePositions,
                Deadline = viewModel.Deadline
            };

            await _offersService.UpdateOfferAsync(offerDto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Offers/Delete/5
        [Authorize(Roles = "HRManager,Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var o = await _offersService.GetOfferByIdAsync(id);
            if (o == null) return NotFound();

            var viewModel = new OfferViewModel
            {
                Id = o.Id,
                Title = o.Title,
                Description = o.Description,
                Department = o.Department,
                Specialization = o.Specialization
            };

            return View(viewModel);
        }

        // POST: Offers/Delete/5
        [Authorize(Roles = "HRManager,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Note: Instead of a hard delete, you could update the Status to 'Archived' here
            await _offersService.DeleteOfferAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,HRManager")]
        [HttpGet]
        public IActionResult Applications(int offerId)
        {
            return RedirectToAction("Index", "Applications", new { offerId = offerId });
        }
    }
}