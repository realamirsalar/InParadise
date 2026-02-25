using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using InParadise.Domain.Entities;

namespace InParadise.Application.Common.Interfaces
{
    public interface IVillaRepository
    {
        IEnumerable<Villa> GetAll(Expression<Func<Villa,bool>>? filter = null , string? includeProperties = null );
        Villa Get(Expression<Func<Villa, bool>> filter, string? includeProperties = null);
        void Insert(Villa entity);
        void Update(Villa entity);
        void Delete(Villa entity);
        void Save();
    }
}
