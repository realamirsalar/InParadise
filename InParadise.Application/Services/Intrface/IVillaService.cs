using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace InParadise.Application.Services.Intrface
{
    public interface IVillaService
    {
        IEnumerable<Villa> GetAllVillas();
        Villa GetVillaById(int id);
        void CreateVilla(Villa villa);
        void UpdateVilla(Villa villa);
        bool DeleteVilla(int id);
    }
}