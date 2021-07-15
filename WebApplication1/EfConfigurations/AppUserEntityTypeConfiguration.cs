using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.EfConfigurations
{
    public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");
            builder.HasKey(e => e.IdUser);
            builder.Property(e => e.IdUser).ValueGeneratedOnAdd();
            builder.Property(e => e.Login).IsRequired();
            builder.Property(e => e.Email).IsRequired();
            builder.Property(e => e.RefreshTocken).IsRequired(false);
            builder.Property(e => e.RefreshTokenExp).IsRequired(false);
            builder.Property(e => e.Password).IsRequired();
            builder.Property(e => e.Salt).IsRequired();
         
        }
    }
}
