using System;
using System.Collections.Generic;

namespace DoorUnlocker.API.Models.Permissions
{
    public class UserPermittedDoorsModel
    {
        public int UserId { get; set; }

        public IList<PermittedDoorModel> PermittedDoors { get; set; }
    }
}