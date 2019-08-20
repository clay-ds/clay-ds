namespace DoorUnlocker.API.Application.Domain.Models
{
    public class Door
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int OfficeId { get; set; }

        public Office Office { get; set; }
    }
}