using InParadise.Application.Common.Interfaces;
using InParadise.Web.Models;
using InParadise.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using InParadise.Application.Common.Utility;
using InParadise.Application.Services.Interface;

namespace InParadise.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVillaService _villaService;

        public HomeController(IVillaService villaService)
        {
            _villaService = villaService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomeVM homeVm = new HomeVM()
            {
                Villas = _villaService.GetAllVillas(includeProperties: "VillaAmenity"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            };
            return View(homeVm);
        }

        [HttpPost]
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
        {
            //Thread.Sleep(2000);


            HomeVM homeVM = new HomeVM()
            {
                CheckInDate = checkInDate,
                Villas = _villaService.GetVillasAvailabilityByDate(nights, checkInDate),
                Nights = nights
            };

            return PartialView("_VillaList", homeVM);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}