using System;

namespace DoorUnlocker.API.Models.Entrance
{
    public class EntranceHistoryModel
    {
        public DateTime EntranceDate { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int DoorId { get; set; }

        public string DoorName { get; set; }
    }
}