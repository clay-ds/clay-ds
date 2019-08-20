using System.Collections.Generic;
using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.Models;

namespace DoorUnlocker.API.Application.Domain.Repositories
{
    public interface IEntranceLogRepository
    {
        Task AddLogAsync(EntranceLogEntry logEntry);

        Task<IEnumerable<EntranceLogEntry>> GetLastForUserAsync(int userId, int count);

        Task<IEnumerable<EntranceLogEntry>> GetLastForDoorAsync(int doorId, int count);
    }
}
