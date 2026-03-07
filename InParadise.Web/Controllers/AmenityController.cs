using InParadise.Application.Common.Interfaces;
using InParadise.Domain.Entities;
using InParadise.Infrastructure.Repository;
using InParadise.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var amenity = _unitOfWork.AmenityRepository.GetAll(includeProperties: "Villa");
            return View(amenity);
        }

        [HttpGet]
        public IActionResult Create()
        {
            AmenityVM amenityVM = new AmenityVM()
            {
                VillaList = _unitOfWork.VillaRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Create(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AmenityRepository.Insert(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "امکان رفاهی ویلای شما با موفقیت ثبت گردید!";
                return RedirectToAction(nameof(Index));
            }

            amenityVM.VillaList = _unitOfWork.VillaRepository.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View(amenityVM);
        }

        [HttpGet]
        public IActionResult Update(int amenityId)
        {
            AmenityVM amenityVM = new AmenityVM
            {
                VillaList = _unitOfWork.VillaRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.AmenityRepository.Get(a => a.Id == amenityId)
            };
            if (amenityVM.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Update(AmenityVM amenityVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.AmenityRepository.Update(amenityVM.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "تغییرات شما با موقفیت اعمال گردید!";
                return RedirectToAction(nameof(Index));
            }

            amenityVM.VillaList = _unitOfWork.VillaRepository.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View(amenityVM);
        }

        [HttpGet]
        public IActionResult Delete(int amenityId)
        {
            AmenityVM? amenityVM = new AmenityVM()
            {
                VillaList = _unitOfWork.VillaRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.AmenityRepository.Get(a => a.Id == amenityId)
            };
            if (amenityVM.Amenity is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(amenityVM);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVM amenityVM)
        {
            Amenity? amenity = _unitOfWork.AmenityRepository.Get(a => a.Id == amenityVM.Amenity.Id);
            if (amenity is not null)
            {
                _unitOfWork.AmenityRepository.Delete(amenity);
                _unitOfWork.Save();
                TempData["success"] = "امکان رفاهی ویلای شما با موفقیت حذف گردید!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View();
        }
    }
}