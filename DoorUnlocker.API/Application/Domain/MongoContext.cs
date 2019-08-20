using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.EntityConfigurations;
using DoorUnlocker.API.Application.Domain.Models;
using DoorUnlocker.API.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DoorUnlocker.API.Application.Domain
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        static MongoContext()
        {
            EntranceLogEntryConfiguration.Register();
        }
        
        public MongoContext(IOptions<MongoOptions> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);

            _database = client.GetDatabase(options.Value.Database);
        }

        public IMongoCollection<EntranceLogEntry> EntranceLogEntries
            => _database.GetCollection<EntranceLogEntry>("EntranceLogEntries");
        
        public void Configure()
        {
            EntranceLogEntryConfiguration.EnsureIndexes(this);
        }
    }
}