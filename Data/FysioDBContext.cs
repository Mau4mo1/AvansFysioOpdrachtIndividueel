using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using Core.DomainServices;
using Microsoft.EntityFrameworkCore;
using System;

namespace Core.Data
{
    public class FysioDBContext : DbContext
    {

        public DbSet<PatientModel> patients { get; set; }
        public DbSet<PersonModel> persons { get; set; }
        public DbSet<TherapistModel> therapists {  get; set; }
        //public DbSet<PatientDossierModel> patientDossiers { get; set; }
        public DbSet<TreatmentModel> treatments { get; set; }
        public FysioDBContext()
        {

        }
        public FysioDBContext(DbContextOptions<FysioDBContext> options) : base(options)
        {

        }
        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TherapistModel>().ToTable("Therapist");
            modelBuilder.Entity<TreatmentModel>().ToTable("Treatment");
            modelBuilder.Entity<PatientModel>().ToTable("Patient");
            modelBuilder.Entity<PersonModel>().ToTable("Person")
                .HasIndex(p => new { p.Email })
                .IsUnique(true);
            modelBuilder.Entity<TeacherModel>().ToTable("Teacher")
                .HasData(
                    new TeacherModel { Id = 1, Email = "mauricederidder@outlook.com", Name = "Maurice de Ridder", BIGNumber = 32, PersonnelNumber = 2 },
                    new TeacherModel { Id = 2, Email = "timdelaater@outlook.com", Name = "Tim de Laater", BIGNumber = 3, PersonnelNumber = 3231 },
                    new TeacherModel { Id = 3, Email = "ricoschouten@outlook.com", Name = "Rico Schouten", BIGNumber = 55, PersonnelNumber = 98721 }
                );
            modelBuilder.Entity<StudentModel>().ToTable("Student")
                .HasData(
                    new StudentModel { Id = 4, Name = "Mees Maske", Email="meesmake@outlook.com", StudentNumber = 321},
                    new StudentModel { Id = 5, Name = "Kevin Verhoeven", Email = "kevin@outlook.com", StudentNumber = 33421 }
                );
           
            //modelBuilder.Entity<PersonModel>().ToTable("PatientDossier");
        }
        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
        }

    }
}
