using System.Collections.Generic;
using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DoorUnlocker.API.Application.Domain.Repositories
{
    public class EntranceLogRepository : IEntranceLogRepository
    {
        private readonly MongoContext _context;

        public EntranceLogRepository(MongoContext context)
        {
            _context = context;
        }
        
        public async Task AddLogAsync(EntranceLogEntry logEntry)
        {
            await _context.EntranceLogEntries.InsertOneAsync(logEntry);
        }

        public async Task<IEnumerable<EntranceLogEntry>> GetLastForUserAsync(int userId, int count)
        {
            return await _context.EntranceLogEntries
                .AsQueryable()
                .OrderByDescending(e => e.EntranceDate)
                .Where(e => e.UserId == userId)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<EntranceLogEntry>> GetLastForDoorAsync(int doorId, int count)
        {
            return await _context.EntranceLogEntries
                .AsQueryable()
                .OrderByDescending(e => e.EntranceDate)
                .Where(e => e.DoorId == doorId)
                .Take(count)
                .ToListAsync();
        }
    }
}