using InParadise.Application.Common.Interfaces;
using InParadise.Web.Models;
using InParadise.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

namespace InParadise.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomeVM homeVm = new HomeVM()
            {
                Villas = _UnitOfWork.VillaRepository.GetAll(includeProperties: "VillaAmenity"),
                Nights = 1,
                CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            };
            return View(homeVm);
        }

        [HttpPost]
        public IActionResult Index(HomeVM homeVm)
        {
            homeVm.Villas = _UnitOfWork.VillaRepository.GetAll(includeProperties: "VillaAmenity");
            foreach (var Villa in homeVm.Villas)
            {
                if (Villa.Id % 2 == 0)
                {
                    Villa.IsAvailable = false;
                }
            }

            return View(homeVm);
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