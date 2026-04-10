using InParadise.Application.Common.Interfaces;
using InParadise.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Application.Services.Interface;

namespace InParadise.Application.Services.Implementation
{
    public class VillaNumberService : IVillaNumberService
    {
        private readonly IUnitOfWork _UnitOfWork;

        public VillaNumberService(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }


        public IEnumerable<VillaNumber> GetAllVillaNumbers(string? includeProperties = null)
        {
            if (!string.IsNullOrEmpty(includeProperties))
            {
                return _UnitOfWork.VillaNumberRepository.GetAll(includeProperties: includeProperties);
            }

            return _UnitOfWork.VillaNumberRepository.GetAll();
        }

        public VillaNumber GetVillaNumberById(int id, string? includeProperties = null)
        {
            if (!string.IsNullOrEmpty(includeProperties))
            {
                return _UnitOfWork.VillaNumberRepository.Get(v => v.NumberOfVilla == id,
                    includeProperties: includeProperties);
            }

            return _UnitOfWork.VillaNumberRepository.Get(v => v.NumberOfVilla == id);
        }

        public void CreateVillaNumber(VillaNumber VillaNumber)
        {
            _UnitOfWork.VillaNumberRepository.Insert(VillaNumber);
            _UnitOfWork.Save();
        }

        public void UpdateVillaNumber(VillaNumber VillaNumber)
        {
            _UnitOfWork.VillaNumberRepository.Update(VillaNumber);
            _UnitOfWork.Save();
        }

        public bool DeleteVillaNumber(int id)
        {
            try
            {
                VillaNumber? dbVillaNumber = _UnitOfWork.VillaNumberRepository.Get(v => v.NumberOfVilla == id);

                if (dbVillaNumber is not null)
                {
                    _UnitOfWork.VillaNumberRepository.Delete(dbVillaNumber);
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

        public bool CheckVillaNumberExist(int villaNumberId)
        {
            return _UnitOfWork.VillaNumberRepository.Any(v => v.NumberOfVilla == villaNumberId);
        }
    }
}