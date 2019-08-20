using System.Collections.Generic;

namespace DoorUnlocker.API.Application.Domain.Models
{
    public class Office
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Door> Doors { get; set; }
    }
}