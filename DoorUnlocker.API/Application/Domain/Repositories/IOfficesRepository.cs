using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.Models;

namespace DoorUnlocker.API.Application.Domain.Repositories
{
    public interface IOfficesRepository
    {
        Task<int> AddAsync(Office office);

        Task<Office> GetAsync(int id);
        
        Task<int> AddDoorAsync(Office office, Door door);

        Task DeleteAsync(Office office);
    }
}