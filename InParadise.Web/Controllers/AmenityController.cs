using InParadise.Application.Common.Interfaces;
using InParadise.Application.Common.Utility;
using InParadise.Application.Services.Interface;
using InParadise.Domain.Entities;
using InParadise.Infrastructure.Repository;
using InParadise.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;

namespace InParadise.Web.Controllers
{
    [Authorize(Roles = SD.AdminRole)]
    public class AmenityController : Controller
    {
        private readonly IAmenityService _amenityService;
        private readonly IVillaService _villaService;

        public AmenityController(IAmenityService amenityService, IVillaService villaService)
        {
            _amenityService = amenityService;
            _villaService = villaService;
        }

        public IActionResult Index()
        {
            var amenity = _amenityService.GetAllAmenities("Villa");
            return View(amenity);
        }

        [HttpGet]
        public IActionResult Create()
        {
            AmenityVM amenityVM = new AmenityVM()
            {
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
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
                _amenityService.CreateAmenity(amenityVM.Amenity);
                TempData["success"] = "امکان رفاهی ویلای شما با موفقیت ثبت گردید!";
                return RedirectToAction(nameof(Index));
            }

            amenityVM.VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
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
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(amenityId)
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
                _amenityService.UpdateAmenity(amenityVM.Amenity);
                TempData["success"] = "تغییرات شما با موقفیت اعمال گردید!";
                return RedirectToAction(nameof(Index));
            }

            amenityVM.VillaList = _amenityService.GetAllAmenities().Select(u => new SelectListItem
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
                VillaList = _villaService.GetAllVillas().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _amenityService.GetAmenityById(amenityId)
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
            var deleted = _amenityService.DeleteAmenity(amenityVM.Amenity.Id);
            if (deleted)
            {
                TempData["success"] = "امکان رفاهی ویلای شما با موفقیت حذف گردید!";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View();
        }
    }
}