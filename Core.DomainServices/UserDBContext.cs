using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace AvansFysioOpdrachtIndividueel.Data
{
    public class UserDBContext : IdentityDbContext
    {
        
       
        public UserDBContext()
        {

        }
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityRole<string>>().HasData(
            new IdentityRole<string>
            {
                Id = "1",
                Name = "Fysiotherapist",
                NormalizedName = "Fysiotherapist",
                ConcurrencyStamp = ""
            },
            new IdentityRole<string>
            {
                Id = "2",
                Name = "Patient",
                NormalizedName = "Patient",
                ConcurrencyStamp = ""
            });
        }
    }
}
