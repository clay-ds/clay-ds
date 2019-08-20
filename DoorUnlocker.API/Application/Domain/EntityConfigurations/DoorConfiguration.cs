using DoorUnlocker.API.Application.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoorUnlocker.API.Application.Domain.EntityConfigurations
{
    public class DoorConfiguration : IEntityTypeConfiguration<Door>
    {
        public void Configure(EntityTypeBuilder<Door> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(d => d.Office)
                .WithMany(o => o.Doors);
        }
    }
}