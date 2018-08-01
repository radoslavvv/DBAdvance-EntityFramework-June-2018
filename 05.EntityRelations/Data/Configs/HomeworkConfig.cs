using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Configs
{
    public class HomeworkConfig : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {
            builder.HasKey(h => h.HomeworkId);

            builder.Property(h => h.Content)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(h => h.ContentType)
                .IsRequired();

            builder.Property(h => h.SubmissionTime)
                .IsRequired();
        }
    }
}
