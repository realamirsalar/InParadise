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
    }
}