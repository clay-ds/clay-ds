using DoorUnlocker.API.Application.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DoorUnlocker.API.Application.Domain.EntityConfigurations
{
    public class DoorPermissionConfiguration : IEntityTypeConfiguration<DoorPermission>
    {
        public void Configure(EntityTypeBuilder<DoorPermission> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.Door)
                .WithMany();
        }
    }
}