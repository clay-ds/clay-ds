namespace DoorUnlocker.API.Application.Domain.Models
{
    public class DoorPermission
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int DoorId { get; set; }

        public Door Door { get; set; }
    }
}