using AvansFysioOpdrachtIndividueel.Models;
using Core.Domain.Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.ApiInfrastructure
{
    public class VektisDBContext : DbContext
    {

        public DbSet<DiagnosisModel> diagnosis { get; set; }
        public DbSet<VektisModel> vekti { get; set; }
        public VektisDBContext()
        {

        }
        public VektisDBContext(DbContextOptions<VektisDBContext> options) : base(options)
        {

        }
        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<PersonModel>().ToTable("PatientDossier");
        }
        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.EnableSensitiveDataLogging();
        }

    }
}
