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
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Therapist", NormalizedName="Therapist"},
                new IdentityRole { Id = "2", Name = "Student", NormalizedName = "Student" },
                new IdentityRole { Id = "3", Name = "Patient", NormalizedName = "Patient" }
                
            );
            modelBuilder.Entity<IdentityUser>().HasData(
                new IdentityUser { Email = "mauricederidder@outlook.com", Id = "1", PasswordHash = "TestPass21", UserName = "mauricederidder@outlook.com" }
            );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = "1", UserId="1"}
            );
        }
    }
}
/*
 *  .HasData(
                    new TeacherModel { Id = 1, Email = "mauricederidder@outlook.com", Name = "Maurice de Ridder", BIGNumber = 32, PersonnelNumber = 2 },
                    new TeacherModel { Id = 2, Email = "timdelaater@outlook.com", Name = "Tim de Laater", BIGNumber = 3, PersonnelNumber = 3231 },
                    new TeacherModel { Id = 3, Email = "ricoschouten@outlook.com", Name = "Rico Schouten", BIGNumber = 55, PersonnelNumber = 98721 }
                );
            modelBuilder.Entity<StudentModel>().ToTable("Student")
                .HasData(
                    new StudentModel { Id = 1, Name = "Mees Maske", Email="meesmake@outlook.com", StudentNumber = 321},
                    new StudentModel { Id = 1, Name = "Kevin Verhoeven", Email = "kevin@outlook.com", StudentNumber = 33421 }
                );*/