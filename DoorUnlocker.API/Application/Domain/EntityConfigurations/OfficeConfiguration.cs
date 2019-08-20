using DoorUnlocker.API.Application.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoorUnlocker.API.Application.Domain.EntityConfigurations
{
    public class OfficeConfiguration : IEntityTypeConfiguration<Office>
    {
        public void Configure(EntityTypeBuilder<Office> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}