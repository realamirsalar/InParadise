using Microsoft.AspNetCore.Mvc;

namespace InParadise.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
