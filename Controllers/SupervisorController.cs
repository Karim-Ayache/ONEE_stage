using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONEE_Stage.Data;
using ONEE_Stage.Models.Entities;

namespace ONEE_Stage.Controllers
{
    [Authorize]
    public class SupervisorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupervisorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Safely handle potential null identity
            var currentUserName = User.Identity?.Name ?? string.Empty;

            var applications = await _context.Applications
                .Include(a => a.Student)
                .Include(a => a.Offer)
                .Include(a => a.AIScore)
                .ToListAsync();

            return View(applications);
        }
    }
}