using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Application.Common.Interfaces;
using InParadise.Domain.Entities;
using InParadise.Infrastructure.Data;

namespace InParadise.Infrastructure.Repository
{
    public class AmenityRepository : Repository<Amenity> , IAmenity
    {
        private readonly ApplicationDbContext _db;
        public AmenityRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public void Update(Amenity entity)
        {
            this._db.Amenities.Update(entity);
        }
    }
}
