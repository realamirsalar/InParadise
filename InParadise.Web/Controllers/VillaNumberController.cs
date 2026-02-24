using InParadise.Domain.Entities;
using InParadise.Infrastructure.Data;
using InParadise.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var villaNumbers = db.VillaNumbers.Include(v => v.Villa);
            return View(villaNumbers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            VillaNumberVM villaNumber = new()
            {
                VillaList = db.Villas.ToList().Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };
            //IEnumerable<SelectListItem> villaList = db.Villas.ToList().Select(v => new SelectListItem()
            //{
            //    Text = v.Name,
            //    Value = v.Id.ToString()
            //});
            //ViewData["list"] = villaList;
            return View(villaNumber);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            //ModelState.Remove("Villa");
            bool roomIsExist = db.VillaNumbers.Any(v => v.NumberOfVilla == obj.VillaNumber.NumberOfVilla);

            if (ModelState.IsValid && !roomIsExist)
            {
                //db.VillaNumbers.Add(villaNumber);
                db.SaveChanges();
                TempData["success"] = "شماره ویلای شما با موفقیت ثبت گردید!";
                return RedirectToAction("Index", "VillaNumber");
            }

            if (roomIsExist)
            {
                TempData["error"] = "شماره ویلایی قبلا با این شماره ثبت شده است!";
            }

            obj.VillaList = db.Villas.ToList().Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });
            return View(obj);
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