using InParadise.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace InParadise.Application.Services.Interface
{
    public interface IAmenityService
    {
        IEnumerable<Amenity> GetAllAmenities(string? includeProperties = null);
        Amenity GetAmenityById(int id, string? includeProperties = null);
        void CreateAmenity(Amenity amenity);
        void UpdateAmenity(Amenity amenity);
        bool DeleteAmenity(int id);
    }
}