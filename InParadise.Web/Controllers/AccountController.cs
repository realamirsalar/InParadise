using InParadise.Application.Common.Interfaces;
using InParadise.Application.Common.Utility;
using InParadise.Domain.Entities;
using InParadise.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InParadise.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            LoginVM loginVm = new LoginVM()
            {
                RedirectUrl = returnUrl
            };
            return View(loginVm);
        }

        public IActionResult Register()
        {
            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).Wait();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).Wait();
            }

            RegisterVM registerVM = new RegisterVM()
            {
                RoleList = _roleManager.Roles.Select(r => new SelectListItem()
                {
                    Text = r.Name,
                    Value = r.Name
                })
            };
            return View(registerVM);
        }
    }
}