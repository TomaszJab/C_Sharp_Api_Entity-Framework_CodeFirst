using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.EfConfigurations;

namespace WebApplication1.Models
{
    public class MainDbContext :DbContext
    {
        public MainDbContext()
        {

        }

        public MainDbContext(DbContextOptions options)
            : base(options){}

        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription_Medicament> Prescription_Medicaments{ get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.
                UseSqlServer("Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True");
        }//Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new PatientEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new DoctorEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new MedicamentEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new Prescription_MedicamentEntityTypeConfiguration());

            modelBuilder.ApplyConfiguration(new PrescriptionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserEntityTypeConfiguration());
        }
    }
}
