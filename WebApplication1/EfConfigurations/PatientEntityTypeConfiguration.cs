using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using WebApplication1.Models;

namespace WebApplication1.EfConfigurations
{
    public class PatientEntityTypeConfiguration : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.ToTable("Patients");//

            builder.HasKey(e => e.IdPatient);

            builder.Property(e => e.FirstName).HasMaxLength(100).IsRequired();

            builder.Property(e => e.LastName).HasMaxLength(100).IsRequired();

            builder.Property(e => e.Birthdate).IsRequired();

            var patients = new List<Patient>();

            patients.Add(new Patient { Birthdate = DateTime.Today, FirstName = "Jakob", LastName = "Toretto", IdPatient = 1 });

            patients.Add(new Patient { Birthdate = DateTime.Today, FirstName = "Michelle", LastName = "Rodriguez", IdPatient = 2 });

            patients.Add(new Patient { Birthdate = DateTime.Today, FirstName = "Tyrese", LastName = "Gibson", IdPatient = 3 });

            builder.HasData(patients);
        }
    }
}
