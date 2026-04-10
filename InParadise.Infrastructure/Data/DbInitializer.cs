using InParadise.Application.Common.Interfaces;
using InParadise.Application.Common.Utility;
using InParadise.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InParadise.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this._db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initializer()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }

                if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).Wait();
                    _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).Wait();

                    _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "amirsalar@gmail.com",
                        Email = "amirsalar@gmail.com",
                        Name = "amirsalar kheiry",
                        NormalizedEmail = "AMIRSALAR@GMAIL.COM",
                        NormalizedUserName = "AMIRSALAR@GMAIL.COM",
                        PhoneNumber = "09036548295",
                    }, "Admin123*").GetAwaiter().GetResult();
                    ApplicationUser user = _db.ApplicationUsers.SingleOrDefault(u => u.Email == "amirsalar@gmail.com");
                    _userManager.AddToRoleAsync(user, SD.AdminRole).GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}