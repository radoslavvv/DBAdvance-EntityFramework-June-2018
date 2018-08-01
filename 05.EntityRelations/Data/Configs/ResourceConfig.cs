using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Configs
{
    public class ResourceConfig : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(r => r.ResourceId);

            builder.Property(r => r.Name)
                .HasMaxLength(50)
                .IsUnicode()
                .IsRequired();

            builder.Property(r => r.Url)
                 .IsUnicode(false)
                .IsRequired();

            builder.Property(r => r.ResourceType)
                .IsRequired();


        }
    }
}
