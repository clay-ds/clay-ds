using System.Collections.Generic;

namespace DoorUnlocker.API.Models.Offices
{
    public class OfficeModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IList<DoorModel> Doors { get; set; } = new List<DoorModel>();
    }
}
