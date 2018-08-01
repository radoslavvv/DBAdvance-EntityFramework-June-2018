using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Configs
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.StudentId);

            builder.Property(s => s.Name)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            builder.Property(s => s.PhoneNumber)
                .IsRequired(false)
                .HasColumnType("CHAR(10)")
                .IsUnicode(false);

            builder.Property(s => s.RegisteredOn)
                .IsRequired();

            builder.Property(s => s.Birthday)
                .IsRequired(false);
        }
    }
}
