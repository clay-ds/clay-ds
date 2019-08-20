using System.Collections.Generic;
using DoorUnlocker.API.Application.Domain.Models;

namespace DoorUnlocker.API.Application.Domain
{
    public static class TestDbSeeder
    {
        public static void Seed(DoorsContext context)
        {
            var tunnelDoor = new Door
            {
                Name = "Tunnel"
            };
            
            var mainDoor = new Door
            {
                Name = "Main"
            };
            
            var office1 = new Office
            {
                Name = "Office 1",
                Doors = new List<Door> { tunnelDoor, mainDoor }
            };

            context.Offices.Add(office1);

            var permissions = new[]
            {
                // user 1 has access to 1 door
                new DoorPermission
                {
                    UserId = 1,
                    Door = mainDoor
                },
                // user 2 has access to 2 doors
                new DoorPermission
                {
                    UserId = 2,
                    Door = mainDoor
                },
                new DoorPermission
                {
                    UserId = 2,
                    Door = tunnelDoor
                },
                // user 3 has no access
                // user 4 is admin
            };
            
            context.DoorPermissions.AddRange(permissions);

            context.SaveChanges();
        }
    }
}