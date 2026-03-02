using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Domain.Entities;

namespace InParadise.Application.Common.Interfaces
{
    public interface IAmenity : IRepository<Amenity>
    {
        public void Update(Amenity entity);
    }
}