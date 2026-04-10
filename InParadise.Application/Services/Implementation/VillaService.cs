using InParadise.Application.Common.Interfaces;
using InParadise.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Application.Services.Interface;

namespace InParadise.Application.Services.Implementation
{
    public class VillaService : IVillaService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public VillaService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _UnitOfWork = unitOfWork;
            _WebHostEnvironment = webHostEnvironment;
        }


        public IEnumerable<Villa> GetAllVillas(string? includeProperties)
        {
            if (!string.IsNullOrEmpty(includeProperties))
            {
                return _UnitOfWork.VillaRepository.GetAll(includeProperties: includeProperties);
            }

            return _UnitOfWork.VillaRepository.GetAll();
        }

        public Villa GetVillaById(int id, string? includeProperties)
        {
            if (!string.IsNullOrEmpty(includeProperties))
            {
                return _UnitOfWork.VillaRepository.Get(v => v.Id == id, includeProperties: includeProperties);
            }

            return _UnitOfWork.VillaRepository.Get(v => v.Id == id);
        }

        public void CreateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_WebHostEnvironment.WebRootPath, @"images\Villa");

                using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                {
                    villa.Image.CopyTo(fileStream);
                }

                villa.ImageUrl = @"\images\Villa\" + fileName;
            }
            else
            {
                villa.ImageUrl = "https://placehold.co/600x400";
            }

            _UnitOfWork.VillaRepository.Insert(villa);
            _UnitOfWork.Save();
        }

        public void UpdateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                string imagePath = Path.Combine(_WebHostEnvironment.WebRootPath, @"images\Villa");

                if (!String.IsNullOrEmpty(villa.ImageUrl))
                {
                    string oldPath = Path.Combine(_WebHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                {
                    villa.Image.CopyTo(fileStream);
                }

                villa.ImageUrl = @"\images\Villa\" + fileName;
            }


            _UnitOfWork.VillaRepository.Update(villa);
            _UnitOfWork.Save();
        }

        public bool DeleteVilla(int id)
        {
            try
            {
                Villa? dbVilla = _UnitOfWork.VillaRepository.Get(v => v.Id == id);

                if (dbVilla is not null)
                {
                    if (!String.IsNullOrEmpty(dbVilla.ImageUrl))
                    {
                        string oldPath = Path.Combine(_WebHostEnvironment.WebRootPath,
                            dbVilla.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    _UnitOfWork.VillaRepository.Delete(dbVilla);
                    _UnitOfWork.Save();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}