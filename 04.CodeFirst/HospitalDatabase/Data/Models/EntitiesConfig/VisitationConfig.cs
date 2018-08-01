using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data.Models.EntitiesConfig
{
    public class VisitationConfig : IEntityTypeConfiguration<Visitation>
    {
        public void Configure(EntityTypeBuilder<Visitation> builder)
        {
            builder.HasKey(x => x.VisitationId);

            builder.Property(x => x.Comments)
                .HasMaxLength(250)
                .IsUnicode();
        }
    }
}
