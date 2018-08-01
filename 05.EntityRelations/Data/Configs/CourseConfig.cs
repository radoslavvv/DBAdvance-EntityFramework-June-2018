using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Configs
{
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.CourseId);

            builder.Property(c => c.Name)
                .HasMaxLength(80)
                .IsUnicode()
                .IsRequired();

            builder.Property(c => c.Description)
                .IsUnicode()
                .IsRequired(false);

            builder.Property(c => c.StartDate)
                .IsRequired();


            builder.Property(c => c.EndDate)
                .IsRequired();

            builder.Property(c => c.Price)
                .IsRequired();

            builder.HasMany(c => c.StudentsEnrolled)
                .WithOne(c => c.Course)
                .HasForeignKey(c => c.CourseId);

            builder.HasMany(c => c.Resources)
                .WithOne(c => c.Course)
                .HasForeignKey(c => c.CourseId);

            builder.HasMany(c => c.HomeworkSubmissions)
                .WithOne(c => c.Course)
                .HasForeignKey(c => c.CourseId);
        }
    }
}
