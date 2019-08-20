using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DoorUnlocker.API.Application.Domain.Repositories
{
    public class DoorsRepository : IDoorsRepository
    {
        private readonly DoorsContext _context;

        public DoorsRepository(DoorsContext context)
        {
            _context = context;
        }
        
        public async Task<Door> GetAsync(int id)
        {
            return await _context.Doors
                .Include(d => d.Office)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> AllExistAsync(IEnumerable<int> ids)
        {
            var doorsCount = await _context.Doors
                .Where(d => ids.Contains(d.Id))
                .CountAsync();

            return doorsCount == ids.Count();
        }
    }
}