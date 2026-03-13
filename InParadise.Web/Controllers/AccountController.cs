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

        [HttpGet]
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

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            ApplicationUser user = new()
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                PhoneNumber = registerVM.PhoneNumber,
                NormalizedEmail = registerVM.Email.ToUpper(),
                EmailConfirmed = true,
                UserName = registerVM.Email,
                CreateAt = DateTime.Now
            };
            //var result = _userManager.CreateAsync(user, registerVM.Password).GetAwaiter().GetResult();
            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(registerVM.Role))
                {
                    await _userManager.AddToRoleAsync(user, registerVM.Role);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, SD.CustomerRole);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return LocalRedirect(registerVM.RedirectUrl);
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            registerVM.RoleList = _roleManager.Roles.Select(r => new SelectListItem()
            {
                Text = r.Name,
                Value = r.Name
            });

            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password,
                    loginVM.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(loginVM.RedirectUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "اطلاعات وارد شده صحیح نمی باشد.");
                }
            }

            return View(loginVM);
        }
    }
}