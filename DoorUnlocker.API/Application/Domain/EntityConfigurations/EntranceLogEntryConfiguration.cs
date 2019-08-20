using System.Threading.Tasks;
using DoorUnlocker.API.Application.Domain.Models;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;

namespace DoorUnlocker.API.Application.Domain.EntityConfigurations
{
    public static class EntranceLogEntryConfiguration
    {
        public static void Register()
        {
            BsonClassMap.RegisterClassMap<EntranceLogEntry>(cm =>
            {
                cm.AutoMap();
                cm.MapIdProperty(e => e.Id).SetIdGenerator(GuidGenerator.Instance);
            });
        }

        public static void EnsureIndexes(MongoContext ctx)
        {
            var dateOrderIndex = new CreateIndexModel<EntranceLogEntry>(
                Builders<EntranceLogEntry>.IndexKeys.Descending(e => e.EntranceDate));
            
            ctx.EntranceLogEntries.Indexes.CreateOne(dateOrderIndex);
        }
    }
}