using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.EfConfigurations
{
    public class Prescription_MedicamentEntityTypeConfiguration : IEntityTypeConfiguration<Prescription_Medicament>
    {
        public void Configure(EntityTypeBuilder<Prescription_Medicament> builder)
        {
            builder.ToTable("Prescription_Medicaments");//

            builder.HasKey(e => new { e.IdPrescription, e.IdMedicament });

            builder.Property(e => e.Details).HasMaxLength(100).IsRequired();

            builder.Property(e => e.Dose).IsRequired(false);

            builder.HasOne(e => e.Prescription)
                  .WithMany(p => p.Prescription_Medicaments)
                  .HasForeignKey(d => d.IdPrescription);



            builder.HasOne(e => e.Medicament)
                 .WithMany(p => p.Prescription_Medicaments)
                 .HasForeignKey(d => d.IdMedicament);



            List<Prescription_Medicament> PrescriptionsMedicaments = new List<Prescription_Medicament>();

            PrescriptionsMedicaments.Add(new Prescription_Medicament { IdPrescription = 1, IdMedicament = 1, Details = "1detail", Dose = 5 });

            PrescriptionsMedicaments.Add(new Prescription_Medicament { IdPrescription = 2, IdMedicament = 2, Details = "2details", Dose = 10 });

            PrescriptionsMedicaments.Add(new Prescription_Medicament { IdPrescription = 3, IdMedicament = 3, Details = "3details", Dose = 15 });

            builder.HasData(PrescriptionsMedicaments);
        }
    }
}
