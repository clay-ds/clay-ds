using System;

namespace DoorUnlocker.API.Models.Permissions
{
    public class PermittedDoorModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid OfficeId { get; set; }

        public string OfficeName { get; set; }
    }
}