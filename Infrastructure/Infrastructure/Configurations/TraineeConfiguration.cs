using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Web.Models.Entities;

namespace Web.Infrastructure.Configurations
{
    public class TraineeConfiguration : IEntityTypeConfiguration<Trainee>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Trainee> builder)
        {
            builder.Property(t => t.UserId)
                  .HasMaxLength(450);

            builder.HasAlternateKey(t => t.UserId);

            builder
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .HasPrincipalKey(u => u.Id);
        }
    }
}
