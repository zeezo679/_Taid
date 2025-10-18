using Demo.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CrsResultConfiguration : IEntityTypeConfiguration<CrsResult>
    {
        public void Configure(EntityTypeBuilder<CrsResult> builder)
        {
       
            builder
                .HasOne(cr => cr.Trainee)
                .WithMany(t => t.CrsResults)
                .HasForeignKey(cr => cr.UserId)
                .HasPrincipalKey(t => t.UserId);
        }
    }

