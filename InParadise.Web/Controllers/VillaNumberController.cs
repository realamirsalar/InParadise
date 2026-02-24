using InParadise.Domain.Entities;
using InParadise.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace InParadise.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext db;

        public VillaNumberController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var villaNumbers = db.VillaNumbers;
            return View(villaNumbers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(VillaNumber villaNumber)
        {
            //ModelState.Remove("Villa");
            if (ModelState.IsValid)
            {
                db.VillaNumbers.Add(villaNumber);
                db.SaveChanges();
                TempData["success"] = "شماره ویلای شما با موفقیت ثبت گردید!";
                return RedirectToAction("Index", "VillaNumber");
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View(villaNumber);
        }

        [HttpGet]
        public IActionResult Update(int VillaId)
        {
            Villa? villa = db.Villas.SingleOrDefault(v => v.Id == VillaId);
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
                db.Villas.Update(villa);
                db.SaveChanges();
                TempData["success"] = "تغییرات شما با موقفیت اعمال گردید!";
                return RedirectToAction("Index", "Villa");
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View(villa);
        }

        [HttpGet]
        public IActionResult Delete(int VillaId)
        {
            Villa? villa = db.Villas.SingleOrDefault(v => v.Id == VillaId);
            if (villa is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? dbVilla = db.Villas.SingleOrDefault(v => v.Id == villa.Id);

            if (dbVilla is not null)
            {
                db.Villas.Remove(dbVilla);
                db.SaveChanges();
                TempData["success"] = "ویلای شما با موفقیت حذف گردید!";
                return RedirectToAction("Index", "Villa");
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View(villa);
        }
    }
}