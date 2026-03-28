using InParadise.Application.Common.Interfaces;
using InParadise.Web.Models;
using InParadise.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using InParadise.Application.Common.Utility;

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
        public IActionResult GetVillasByDate(int nights, DateOnly checkInDate)
        {
            //Thread.Sleep(2000);
            var villas = _UnitOfWork.VillaRepository.GetAll(includeProperties: "VillaAmenity");
            var villaNumbersList = _UnitOfWork.VillaNumberRepository.GetAll().ToList();
            var bookedVillas = _UnitOfWork.Booking
                .GetAll(b => b.Status == SD.StatusApproved || b.Status == SD.StatusCheckedIn).ToList();

            foreach (var Villa in villas)
            {
                int roomsAvailable =
                    SD.VillaRoomsAvailableCount(Villa.Id, villaNumbersList, checkInDate, nights, bookedVillas);

                Villa.IsAvailable = roomsAvailable > 0 ? true : false;
            }

            HomeVM homeVM = new HomeVM()
            {
                CheckInDate = checkInDate,
                Villas = villas,
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