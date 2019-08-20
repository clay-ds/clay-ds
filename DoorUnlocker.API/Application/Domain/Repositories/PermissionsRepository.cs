using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DoorUnlocker.API.Application.Domain.Repositories
{
    public class PermissionsRepository : IPermissionsRepository
    {
        private readonly DoorsContext _context;

        public PermissionsRepository(DoorsContext context)
        {
            _context = context;
        }
        
        public async Task<IList<DoorPermission>> GetDoorPermissionsAsync(int userId)
        {
            return await _context.DoorPermissions
                .Include(p => p.Door)
                .ThenInclude(d => d.Office)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateDoorPermissionsAsync(UpdateDoorPermissionsModel permissions)
        {
            _context.DoorPermissions.RemoveRange(permissions.ToDelete);
            _context.DoorPermissions.AddRange(permissions.ToAdd);

            await _context.SaveChangesAsync();
        }
    }
}