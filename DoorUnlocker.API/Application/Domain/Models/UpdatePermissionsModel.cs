using System.Collections.Generic;

namespace DoorUnlocker.API.Application.Domain.Models
{
    public class UpdateDoorPermissionsModel
    {
        public IEnumerable<DoorPermission> ToDelete { get; set; }

        public IEnumerable<DoorPermission> ToAdd { get; set; }
    }
}
