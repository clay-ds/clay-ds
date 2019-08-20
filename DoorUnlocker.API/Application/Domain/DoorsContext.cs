using DoorUnlocker.API.Application.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DoorUnlocker.API.Application.Domain
{
    public class DoorsContext : DbContext
    {
        public DoorsContext(DbContextOptions<DoorsContext> options)
            : base(options)
        {
        }

        public DbSet<Door> Doors { get; set; }

        public DbSet<Office> Offices { get; set; }
        
        public DbSet<DoorPermission> DoorPermissions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}