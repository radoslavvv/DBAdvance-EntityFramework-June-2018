﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data.Models.EntitiesConfig
{
    public class MedicamentConfig : IEntityTypeConfiguration<Medicament>
    {
        public void Configure(EntityTypeBuilder<Medicament> builder)
        {
            builder.HasKey(x => x.MedicamentId);

            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode();

            builder.HasMany(x => x.Prescriptions)
                .WithOne(x => x.Medicament)
                .HasForeignKey(x => x.MedicamentId);
        }
    }
}
