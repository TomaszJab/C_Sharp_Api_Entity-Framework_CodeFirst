using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.EfConfigurations
{
    public class DoctorEntityTypeConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.ToTable("Doctors");//

            builder.HasKey(e => e.IdDoctor);

            builder.Property(e => e.IdDoctor).ValueGeneratedOnAdd();//

            builder.Property(e => e.FirstName).HasMaxLength(100).IsRequired();

            builder.Property(e => e.LastName).HasMaxLength(100).IsRequired();

            builder.Property(e => e.Email).HasMaxLength(100).IsRequired();


            var doctors = new List<Doctor>();

            doctors.Add(new Doctor { IdDoctor = 1, FirstName = "Tomasz", LastName = "J", Email = "TomaszJ@gmail.com" });

            doctors.Add(new Doctor { IdDoctor = 2, FirstName = "Johnny", LastName = "Crash", Email = "JohnnyCrash@gmail.com" });

            doctors.Add(new Doctor { IdDoctor = 3, FirstName = "Vin", LastName = "Diesel", Email = "VinDiesel@gmail.com" });

            builder.HasData(doctors);
        }
    }
}
