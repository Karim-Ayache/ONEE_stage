using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONEE_Stage.Data;
using ONEE_Stage.Models.Entities;
using ONEE_Stage.Models.Enums;
using ONEE_Stage.Models.ViewModels;

namespace ONEE_Stage.Controllers
{
    [Authorize(Roles = "Admin,HRManager")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string ONEEEmailRegex = @"^[a-zA-Z0-9._%+-]+@onee\.(ma|org\.ma)$";

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // HR & Admin Review Dashboard routing
        [HttpGet]
        public async Task<IActionResult> Applications(int? offerId)
        {
            if (!offerId.HasValue)
            {
                var firstOffer = await _context.Offers.OrderByDescending(o => o.Id).FirstOrDefaultAsync();
                if (firstOffer == null) return RedirectToAction("Index", "Offers");
                offerId = firstOffer.Id;
            }

            // Redirect to the actual applications review controller/action
            return RedirectToAction("Index", "Applications", new { offerId = offerId.Value });
        }

        // --- ADMIN ONLY: Register HR Manager ---
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult RegisterHRManager() => View("RegisterHRManager");

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterHRManager(RegisterHRManagerViewModel model)
        {
            if (!Regex.IsMatch(model.Email ?? string.Empty, ONEEEmailRegex, RegexOptions.IgnoreCase))
            {
                ModelState.AddModelError("Email", "Registration restricted to official ONEE emails (@onee.ma).");
            }

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
            }

            if (!ModelState.IsValid) return View("RegisterHRManager", model);

            var hrManager = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = UserRole.HRManager,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(hrManager);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"HR Manager account for {model.FirstName} {model.LastName} created successfully.";
            return RedirectToAction("Applications", "Admin");
        }

        // --- HR MANAGER & ADMIN: Register Supervisor ---
        [Authorize(Roles = "HRManager,Admin")]
        [HttpGet]
        public IActionResult RegisterSupervisor() => View("RegisterSupervisor");

        [Authorize(Roles = "HRManager,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSupervisor(RegisterSupervisorViewModel model)
        {
            if (!Regex.IsMatch(model.Email ?? string.Empty, ONEEEmailRegex, RegexOptions.IgnoreCase))
            {
                ModelState.AddModelError("Email", "Registration restricted to official ONEE emails (@onee.ma).");
            }

            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
            }

            if (!ModelState.IsValid) return View("RegisterSupervisor", model);

            var supervisor = new User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = UserRole.Supervisor,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(supervisor);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Supervisor account for {model.FirstName} {model.LastName} created successfully.";
            return RedirectToAction("Applications", "Admin");
        }
    }
}