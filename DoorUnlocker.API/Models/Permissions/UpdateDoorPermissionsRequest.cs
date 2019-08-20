using System;
using System.Collections.Generic;

namespace DoorUnlocker.API.Models.Permissions
{
    public class UpdateDoorPermissionsRequest
    {
        public IList<int> PermittedDoors { get; set; }
    }
}