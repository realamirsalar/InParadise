using InParadise.Application.Common.Interfaces;
using InParadise.Domain.Entities;
using InParadise.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InParadise.Infrastructure.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext db;

        public VillaRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public void Delete(Villa entity)
        {
            db.Remove(entity);
        }

        public Villa Get(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
        {
            IQueryable<Villa> query = db.Set<Villa>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!String.IsNullOrEmpty(includeProperties))
            {
                //Villa -- case sensitive
                foreach (var includeItem in includeProperties.Split(new char[] { ',' }, StringSplitOptions.None))
                {
                    query = query.Include(includeItem);
                }
            }

            return query.SingleOrDefault();
        }

        public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Villa> query = db.Set<Villa>();
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!String.IsNullOrEmpty(includeProperties))
            {
                //Villa -- case sensitive
                foreach (var includeItem in includeProperties.Split(new char[] { ',' }, StringSplitOptions.None))
                {
                    query = query.Include(includeItem);
                }
            }

            return query.ToList();
        }

        public void Insert(Villa entity)
        {
            db.Add(entity);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Villa entity)
        {
            db.Update(entity);
        }
    }
}