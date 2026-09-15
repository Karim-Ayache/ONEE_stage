using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ONEE_Stage.Models.Enums;
using ONEE_Stage.Models.ViewModels;
using ONEE_Stage.Services;

namespace ONEE_Stage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOffersService _offersService;

        public HomeController(ILogger<HomeController> logger, IOffersService offersService)
        {
            _logger = logger;
            _offersService = offersService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Accueil - Plateforme de Gestion des Stages ONEE";

            var offers = await _offersService.GetAllOffersAsync();

            var publishedOffers = offers
                .Where(o => o.Status == OfferStatus.Published)
                .Select(o => new OfferViewModel
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
                })
                .ToList();

            var viewModel = new HomeViewModel
            {
                FeaturedOffers = publishedOffers,
                TotalOpenOffers = publishedOffers.Count
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}