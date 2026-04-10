using InParadise.Application.Common.Interfaces;
using InParadise.Application.Services.Intrface;
using InParadise.Domain.Entities;
using InParadise.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InParadise.Web.Controllers
{
    [Authorize]
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;

        public VillaController(IVillaService villaService)
        {
            _villaService = villaService;
        }

        public IActionResult Index()
        {
            var villas = _villaService.GetAllVillas();
            return View(villas);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa villa)
        {
            if (ModelState.IsValid)
            {
                _villaService.CreateVilla(villa);
                TempData["success"] = "ویلای شما با موفقیت ثبت گردید!";
                return RedirectToAction("Index", "Villa");
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View(villa);
        }

        [HttpGet]
        public IActionResult Update(int VillaId)
        {
            Villa? villa = _villaService.GetVillaById(VillaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (ModelState.IsValid && villa.Id > 0)
            {
                _villaService.UpdateVilla(villa);
                TempData["success"] = "تغییرات شما با موقفیت اعمال گردید!";
                return RedirectToAction("Index", "Villa");
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View(villa);
        }

        [HttpGet]
        public IActionResult Delete(int VillaId)
        {
            Villa? villa = _villaService.GetVillaById(VillaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            var deleted = _villaService.DeleteVilla(villa.Id);
            if (deleted)
            {
                TempData["success"] = "ویلای شما با موفقیت حذف گردید!";
                return RedirectToAction("Index", "Villa");
            }
            else
            {
                TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
                return View(villa);
            }
        }
    }
}