using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data.Models.EntitiesConfig
{
    public class DiagnoseConfig : IEntityTypeConfiguration<Diagnose>
    {
        public void Configure(EntityTypeBuilder<Diagnose> builder)
        {
            builder.HasKey(x => x.DiagnoseId);

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode();

            builder.Property(x => x.Comments)
                .HasMaxLength(250)
                .IsUnicode();
        }
    }
}
