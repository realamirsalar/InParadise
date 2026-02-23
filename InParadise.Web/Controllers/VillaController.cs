using InParadise.Domain.Entities;
using InParadise.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace InParadise.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext db;

        public VillaController(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            var villas = db.Villas;
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
                db.Villas.Add(villa);
                db.SaveChanges();
                return RedirectToAction("Index", "Villa");
            }

            return View(villa);
        }

        [HttpGet]
        public IActionResult Update(int VillaId)
        {
            Villa? villa = db.Villas.SingleOrDefault(v => v.Id == VillaId);
            if (villa == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villa);
        }
    }
}