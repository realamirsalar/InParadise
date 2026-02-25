using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Application.Common.Interfaces;
using InParadise.Domain.Entities;
using InParadise.Infrastructure.Data;

namespace InParadise.Infrastructure.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(VillaNumber entity)
        {
            _db.VillaNumbers.Update(entity);
        }
    }
}