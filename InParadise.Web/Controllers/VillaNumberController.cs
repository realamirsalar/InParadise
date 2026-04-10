using InParadise.Application.Common.Interfaces;
using InParadise.Application.Services.Interface;
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
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;


        public VillaNumberController(IVillaNumberService villaNumbrerService, IVillaService villaService)
        {
            _villaNumberService = villaNumbrerService;
            _villaService = villaService;
        }

        public IActionResult Index()
        {
            var villaNumbers = _villaNumberService.GetAllVillaNumbers("Villa");
            return View(villaNumbers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            VillaNumberVM villaNumber = new()
            {
                VillaList = _villaService.GetAllVillas().ToList().Select(v => new SelectListItem()
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
            bool roomIsExist = _villaNumberService.CheckVillaNumberExist(obj.VillaNumber.NumberOfVilla);

            if (ModelState.IsValid && !roomIsExist)
            {
                //db.VillaNumbers.Add(villaNumber);
                _villaNumberService.CreateVillaNumber(obj.VillaNumber);
                TempData["success"] = "شماره ویلای شما با موفقیت ثبت گردید!";
                return RedirectToAction("Index", "VillaNumber");
            }

            if (roomIsExist)
            {
                TempData["error"] = "شماره ویلایی قبلا با این شماره ثبت شده است!";
            }

            obj.VillaList = _villaService.GetAllVillas().Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });
            return View(obj);
        }

        [HttpGet]
        public IActionResult Update(int VillaNumberId)
        {
            VillaNumberVM villaNumber = new()
            {
                VillaList = _villaService.GetAllVillas().Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _villaNumberService.GetVillaNumberById(VillaNumberId)
            };
            if (villaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumber);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _villaNumberService.UpdateVillaNumber(villaNumberVM.VillaNumber);
                TempData["success"] = "تغییرات شما با موقفیت اعمال گردید!";
                return RedirectToAction("Index", "VillaNumber");
            }

            villaNumberVM.VillaList = _villaService.GetAllVillas().Select(v => new SelectListItem()
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });
            return View(villaNumberVM);
        }

        [HttpGet]
        public IActionResult Delete(int VillaNumberId)
        {
            VillaNumberVM villaNumber = new()
            {
                VillaList = _villaService.GetAllVillas().Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _villaNumberService.GetVillaNumberById(VillaNumberId)
            };
            if (villaNumber is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumber);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNuberNumberVm)
        {
            VillaNumber? dbVillanumber =
                _villaNumberService.GetVillaNumberById(villaNuberNumberVm.VillaNumber.NumberOfVilla);

            if (dbVillanumber is not null)
            {
                _villaNumberService.DeleteVillaNumber(dbVillanumber.NumberOfVilla);
                TempData["success"] = "ویلای شما با موفقیت حذف گردید!";
                return RedirectToAction("Index", "VillaNumber");
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View();
        }
    }
}