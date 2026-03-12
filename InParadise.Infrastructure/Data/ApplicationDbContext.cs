using System;
using System.Collections.Generic;
using System.Text;
using InParadise.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InParadise.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //turn on Identity table keys
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "ویلای رویال",
                    Description =
                        "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    ImageUrl = "https://placehold.co/600x400",
                    Occupancy = 4,
                    Price = 200,
                    Sqft = 550,
                },
                new Villa
                {
                    Id = 2,
                    Name = "ویلای استخردار ممتاز",
                    Description =
                        "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    ImageUrl = "https://placehold.co/600x401",
                    Occupancy = 4,
                    Price = 300,
                    Sqft = 550,
                },
                new Villa
                {
                    Id = 3,
                    Name = "ویلای لوکس با استخر",
                    Description =
                        "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                    ImageUrl = "https://placehold.co/600x402",
                    Occupancy = 4,
                    Price = 400,
                    Sqft = 750,
                });
            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber()
                {
                    NumberOfVilla = 101,
                    VillaId = 1
                },
                new VillaNumber()
                {
                    NumberOfVilla = 102,
                    VillaId = 1
                },
                new VillaNumber()
                {
                    NumberOfVilla = 103,
                    VillaId = 1
                },
                new VillaNumber()
                {
                    NumberOfVilla = 201,
                    VillaId = 2
                },
                new VillaNumber()
                {
                    NumberOfVilla = 202,
                    VillaId = 2
                },
                new VillaNumber()
                {
                    NumberOfVilla = 203,
                    VillaId = 2
                },
                new VillaNumber()
                {
                    NumberOfVilla = 301,
                    VillaId = 3
                },
                new VillaNumber()
                {
                    NumberOfVilla = 302,
                    VillaId = 3
                },
                new VillaNumber()
                {
                    NumberOfVilla = 303,
                    VillaId = 3
                }
            );
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity()
                {
                    Id = 1,
                    VillaId = 1,
                    Name = "استخر خصوصی"
                },
                new Amenity()
                {
                    Id = 2,
                    VillaId = 1,
                    Name = "فر آشپزخانه"
                },
                new Amenity()
                {
                    Id = 3,
                    VillaId = 1,
                    Name = "بالکن خصوصی"
                },
                new Amenity()
                {
                    Id = 4,
                    VillaId = 1,
                    Name = "یک تخت بزرگ به همراه یک مبل تختخواب شو"
                },
                new Amenity()
                {
                    Id = 5,
                    VillaId = 2,
                    Name = "استخز اختصاصی با سقوط آزاد"
                },
                new Amenity()
                {
                    Id = 6,
                    VillaId = 2,
                    Name = "آشپز خانه مجهز"
                },
                new Amenity()
                {
                    Id = 7,
                    VillaId = 2,
                    Name = "بالکن اختصاصی"
                },
                new Amenity()
                {
                    Id = 8,
                    VillaId = 2,
                    Name = "تخت دو نفره"
                },
                new Amenity()
                {
                    Id = 9,
                    VillaId = 3,
                    Name = "استخر خصوصی"
                },
                new Amenity()
                {
                    Id = 10,
                    VillaId = 3,
                    Name = "جکوزی"
                },
                new Amenity()
                {
                    Id = 11,
                    VillaId = 3,
                    Name = "بالکن اختصاصی"
                }
            );
        }
    }
}