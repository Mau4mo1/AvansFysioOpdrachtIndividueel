using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Core.Data
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

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Therapist", NormalizedName="Therapist"},
                new IdentityRole { Id = "2", Name = "Student", NormalizedName = "Student" },
                new IdentityRole { Id = "3", Name = "Patient", NormalizedName = "Patient" }
            );

            var hasher = new PasswordHasher<IdentityUser>();

            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser { Email = "mauricederidder@outlook.com", NormalizedEmail= "mauricederidder@outlook.com", Id = "1", PasswordHash= hasher.HashPassword(null, "TestPass21@"), UserName = "mauricederidder@outlook.com" }
            );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "1", UserId="1"}
            );
        }
    }
}