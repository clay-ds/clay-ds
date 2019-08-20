using System.Collections.Generic;
using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.Models;

namespace DoorUnlocker.API.Application.Domain.Repositories
{
    public interface IPermissionsRepository
    {
        Task<IList<DoorPermission>> GetDoorPermissionsAsync(int userId);

        Task UpdateDoorPermissionsAsync(UpdateDoorPermissionsModel permissions);
    }
}
