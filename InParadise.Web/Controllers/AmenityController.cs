using InParadise.Application.Common.Interfaces;
using InParadise.Domain.Entities;
using InParadise.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace InParadise.Web.Controllers
{
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var amenity = _unitOfWork.AmenityRepository.GetAll();
            return View(amenity);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Amenity amenity)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AmenityRepository.Insert(amenity);
                _unitOfWork.Save();
                TempData["success"] = "ویلای شما با موفقیت ثبت گردید!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View(amenity);
        }

        [HttpGet]
        public IActionResult Update(int amenityId)
        {
            Amenity? amenity = _unitOfWork.AmenityRepository.Get(a => a.Id == amenityId);
            if (amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenity);
        }

        [HttpPost]
        public IActionResult Update(Amenity amenity)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AmenityRepository.Update(amenity);
                _unitOfWork.Save();
                TempData["success"] = "تغییرات شما با موقفیت اعمال گردید!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int amenityId)
        {
            Amenity? amenity = _unitOfWork.AmenityRepository.Get(a => a.Id == amenityId);
            if (amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenity);
        }

        [HttpPost]
        public IActionResult Delete(Amenity amenity)
        {
            Amenity? Amenity = _unitOfWork.AmenityRepository.Get(a => a.Id == amenity.Id);
            if (Amenity is not null)
            {
                _unitOfWork.AmenityRepository.Delete(Amenity);
                _unitOfWork.Save();
                TempData["success"] = "ویلای شما با موفقیت حذف گردید!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View();
        }
    }
}