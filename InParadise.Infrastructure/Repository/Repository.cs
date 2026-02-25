using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using InParadise.Application.Common.Interfaces;

namespace InParadise.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }
    }
}