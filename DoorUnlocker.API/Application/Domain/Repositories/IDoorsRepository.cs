using System.Collections.Generic;
using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.Models;

namespace DoorUnlocker.API.Application.Domain.Repositories
{
    public interface IDoorsRepository
    {
        Task<Door> GetAsync(int id);

        Task<bool> AllExistAsync(IEnumerable<int> ids);
    }
}