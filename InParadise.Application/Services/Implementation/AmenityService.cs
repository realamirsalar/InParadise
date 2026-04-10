using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Application.Common.Interfaces;
using InParadise.Application.Services.Interface;
using InParadise.Domain.Entities;

namespace InParadise.Application.Services.Implementation
{
    public class AmenityService : IAmenityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Amenity> GetAllAmenities(string? includeProperties = null)
        {
            return _unitOfWork.AmenityRepository.GetAll(includeProperties: includeProperties);
        }

        public Amenity GetAmenityById(int id, string? includeProperties = null)
        {
            return _unitOfWork.AmenityRepository.Get(amenity => amenity.Id == id);
        }

        public void CreateAmenity(Amenity amenity)
        {
            _unitOfWork.AmenityRepository.Insert(amenity);
            _unitOfWork.Save();
        }

        public void UpdateAmenity(Amenity amenity)
        {
            _unitOfWork.AmenityRepository.Update(amenity);
            _unitOfWork.Save();
        }

        public bool DeleteAmenity(int id)
        {
            try
            {
                Amenity? amenity = _unitOfWork.AmenityRepository.Get(a => a.Id == id);
                if (amenity is not null)
                {
                    _unitOfWork.AmenityRepository.Delete(amenity);
                    _unitOfWork.Save();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}