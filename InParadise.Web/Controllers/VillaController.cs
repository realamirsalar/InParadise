using InParadise.Application.Common.Interfaces;
using InParadise.Domain.Entities;
using InParadise.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace InParadise.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villas = _UnitOfWork.VillaRepository.GetAll();
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
                _UnitOfWork.VillaRepository.Insert(villa);
                _UnitOfWork.Save();
                TempData["success"] = "ویلای شما با موفقیت ثبت گردید!";
                return RedirectToAction("Index", "Villa");
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View(villa);
        }

        [HttpGet]
        public IActionResult Update(int VillaId)
        {
            Villa? villa = _UnitOfWork.VillaRepository.Get(v => v.Id == VillaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.VillaRepository.Update(villa);
                _UnitOfWork.Save();
                TempData["success"] = "تغییرات شما با موقفیت اعمال گردید!";
                return RedirectToAction("Index", "Villa");
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View(villa);
        }

        [HttpGet]
        public IActionResult Delete(int VillaId)
        {
            Villa? villa = _UnitOfWork.VillaRepository.Get(v => v.Id == VillaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? dbVilla = _UnitOfWork.VillaRepository.Get(v => v.Id == villa.Id);

            if (dbVilla is not null)
            {
                _UnitOfWork.VillaRepository.Delete(dbVilla);
                _UnitOfWork.Save();
                TempData["success"] = "ویلای شما با موفقیت حذف گردید!";
                return RedirectToAction("Index", "Villa");
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View(villa);
        }
    }
}