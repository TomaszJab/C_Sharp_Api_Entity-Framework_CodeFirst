using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.EfConfigurations
{
    public class MedicamentEntityTypeConfiguration : IEntityTypeConfiguration<Medicament>
    {
        public void Configure(EntityTypeBuilder<Medicament> builder)
        {
            builder.ToTable("Medicaments");//

            builder.HasKey(e => e.IdMedicament);

            builder.Property(e => e.IdMedicament).ValueGeneratedOnAdd();//

            builder.Property(e => e.Name).HasMaxLength(100).IsRequired();

            builder.Property(e => e.Description).HasMaxLength(100).IsRequired();

            builder.Property(e => e.Type).HasMaxLength(100).IsRequired();


            var medicaments = new List<Medicament>();

            medicaments.Add(new Medicament { IdMedicament = 1, Name = "Aleric", Description = "aaa", Type = "Aleric" });

            medicaments.Add(new Medicament { IdMedicament = 2, Name = "Alerzina", Description = "bbb", Type = "Aleric" });

            medicaments.Add(new Medicament { IdMedicament = 3, Name = "Avamys", Description = "ccc", Type = "Aleric" });

            builder.HasData(medicaments);
        }
    }
}
