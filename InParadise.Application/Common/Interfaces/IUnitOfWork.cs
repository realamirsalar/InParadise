using System;
using System.Collections.Generic;
using System.Text;

namespace InParadise.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        public IVillaRepository VillaRepository { get; }
        public IVillaNumberRepository VillaNumberRepository { get; }
        public IAmenity Amenity { get; set; }
        void Save();
    }
}