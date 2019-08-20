using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DoorUnlocker.API.Application.Domain.Repositories
{
    public class OfficesRepository : IOfficesRepository
    {
        private readonly DoorsContext _context;

        public OfficesRepository(DoorsContext context)
        {
            _context = context;
        }
        
        public async Task<int> AddAsync(Office office)
        {
            _context.Add(office);
            await _context.SaveChangesAsync();

            return office.Id;
        }

        public async Task<Office> GetAsync(int id)
        {
            return await _context.Offices
                .Include(o => o.Doors)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<int> AddDoorAsync(Office office, Door door)
        {
            office.Doors.Add(door);
            await _context.SaveChangesAsync();

            return door.Id;
        }

        public async Task DeleteAsync(Office office)
        {
            _context.Offices.Remove(office);

            await _context.SaveChangesAsync();
        }
    }
}