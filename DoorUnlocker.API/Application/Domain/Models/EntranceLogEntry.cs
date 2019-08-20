using System;

namespace DoorUnlocker.API.Application.Domain.Models
{
    public class EntranceLogEntry
    {
        public Guid Id { get; set; }
        
        public DateTime EntranceDate { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int DoorId { get; set; }

        public string DoorName { get; set; }
    }
}