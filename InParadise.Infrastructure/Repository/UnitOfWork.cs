using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Application.Common.Interfaces;
using InParadise.Infrastructure.Data;

namespace InParadise.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            VillaRepository = new VillaRepository(_db);
            User = new ApplicationUserRepository(_db);
            VillaNumberRepository = new VillaNumberRepository(_db);
            AmenityRepository = new AmenityRepository(_db);
            Booking = new BookingRepository(_db);
        }

        public IVillaRepository VillaRepository { get; }
        public IVillaNumberRepository VillaNumberRepository { get; }
        public IBookingRepository Booking { get; }
        public IAmenity AmenityRepository { get; }
        public IApplicationUserRepository User { get; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}