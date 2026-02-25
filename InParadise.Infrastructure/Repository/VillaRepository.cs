using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using InParadise.Application.Common.Interfaces;
using InParadise.Domain.Entities;

namespace InParadise.Infrastructure.Repository
{
    public class VillaRepository : IVillaRepository
    {
        public void Delete(Villa entity)
        {
            throw new NotImplementedException();
        }

        public Villa Get(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Insert(Villa entity)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Villa entity)
        {
            throw new NotImplementedException();
        }
    }
}