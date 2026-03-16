using System;
using System.Collections.Generic;
using System.Text;

namespace InParadise.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        public IVillaRepository VillaRepository { get; }
        public IVillaNumberRepository VillaNumberRepository { get; }
        public IBookingRepository Booking { get; }
        public IAmenity AmenityRepository { get; set; }
        void Save();
    }
}