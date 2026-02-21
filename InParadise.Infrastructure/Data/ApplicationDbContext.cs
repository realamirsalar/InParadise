using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InParadise.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Villa> Villas { get; set; }
    }
}