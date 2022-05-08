using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectX.DataAccess.Context.Base;

namespace ProjectX.Tests.Core.Extensions
{
    public static class MongoContextExtensions
    {
        private const string RolesCollectionName = "Roles";
        
        public static Task DeleteDatabase(this IMongoContext mongoContext)
        {
            return mongoContext.Client.DropDatabaseAsync(mongoContext.Settings.DatabaseName);
        }

        public static async Task ClearCollectionsAfterTestRun(this IMongoContext mongoContext)
        {
            var collectionNames = (await mongoContext.Database.ListCollectionNamesAsync()).ToEnumerable()
                .Except(new[] { RolesCollectionName });

            Task DeleteCollectionAction(string name)
            {
                var collection = mongoContext.Database.GetCollection<BsonDocument>(name);
                return collection.DeleteManyAsync(Builders<BsonDocument>.Filter.Empty);
            }

            foreach (var collectionName in collectionNames)
            {
                await DeleteCollectionAction(collectionName);
            }
        }
    }
}