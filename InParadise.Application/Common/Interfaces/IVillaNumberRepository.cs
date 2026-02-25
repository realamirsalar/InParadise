using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Domain.Entities;

namespace InParadise.Application.Common.Interfaces
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        void Update(VillaNumber entity);
    }
}