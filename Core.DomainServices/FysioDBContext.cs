using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AvansFysioOpdrachtIndividueel.Data
{
    public class FysioDBContext : DbContext
    {
        
        public DbSet<PatientModel> patients { get; set; }
        public DbSet<PersonModel> persons { get; set; }
        //public DbSet<PatientDossierModel> patientDossiers { get; set; }
       
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
            modelBuilder.Entity<PatientModel>().ToTable("Patient");
            modelBuilder.Entity<PersonModel>().ToTable("Person");
            //modelBuilder.Entity<PersonModel>().ToTable("PatientDossier");
        }
        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

    }
}
