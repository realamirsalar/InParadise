using InParadise.Application.Common.Interfaces;
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
        private readonly IUnitOfWork _UnitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villaNumbers = _UnitOfWork.VillaNumberRepository.GetAll(null, "Villa");
            return View(villaNumbers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            VillaNumberVM villaNumber = new()
            {
                VillaList = _UnitOfWork.VillaRepository.GetAll().ToList().Select(v => new SelectListItem()
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
            bool roomIsExist = _UnitOfWork.VillaNumberRepository
                .Any(v => v.NumberOfVilla == obj.VillaNumber.NumberOfVilla);

            if (ModelState.IsValid && !roomIsExist)
            {
                //db.VillaNumbers.Add(villaNumber);
                _UnitOfWork.VillaNumberRepository.Insert(obj.VillaNumber);
                _UnitOfWork.Save();
                TempData["success"] = "شماره ویلای شما با موفقیت ثبت گردید!";
                return RedirectToAction("Index", "VillaNumber");
            }

            if (roomIsExist)
            {
                TempData["error"] = "شماره ویلایی قبلا با این شماره ثبت شده است!";
            }

            obj.VillaList = _UnitOfWork.VillaRepository.GetAll().Select(v => new SelectListItem()
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
                VillaList = _UnitOfWork.VillaRepository.GetAll().Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _UnitOfWork.VillaNumberRepository.Get(vn => vn.NumberOfVilla == VillaNumberId)
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
                _UnitOfWork.VillaNumberRepository.Update(villaNumberVM.VillaNumber);
                _UnitOfWork.Save();
                TempData["success"] = "تغییرات شما با موقفیت اعمال گردید!";
                return RedirectToAction("Index", "VillaNumber");
            }

            villaNumberVM.VillaList = _UnitOfWork.VillaRepository.GetAll().Select(v => new SelectListItem()
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
                VillaList = _UnitOfWork.VillaRepository.GetAll().Select(v => new SelectListItem()
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                }),
                VillaNumber = _UnitOfWork.VillaNumberRepository.Get(vn => vn.NumberOfVilla == VillaNumberId)
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
                _UnitOfWork.VillaNumberRepository.Get(v =>
                    v.NumberOfVilla == villaNuberNumberVm.VillaNumber.NumberOfVilla);

            if (dbVillanumber is not null)
            {
                _UnitOfWork.VillaNumberRepository.Delete(dbVillanumber);
                _UnitOfWork.Save();
                TempData["success"] = "ویلای شما با موفقیت حذف گردید!";
                return RedirectToAction("Index", "VillaNumber");
            }

            TempData["error"] = "عملیات ناموفق بود لطفا مجددا اقدام نمایید";
            return View();
        }
    }
}