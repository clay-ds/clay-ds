namespace DoorUnlocker.API.Infrastructure.Configuration
{
    public class IdentityServerOptions
    {
        public string Authority { get; set; }

        public bool RequireHttpsMetadata { get; set; }

        public string Audience { get; set; }
    }
}