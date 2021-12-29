using System.Threading.Tasks;
using MongoDB.Driver;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Context
{
    public class MongoDbContext : IMongoContext
    {
        public IMongoClient Client { get; set; }
        public IMongoDatabase Database { get; set; }
        
        public IDatabaseSettings Settings { get; set; }

        public MongoDbContext(IDatabaseSettings settings)
        {
            Client = new MongoClient(settings.ConnectionString);
            Database = Client.GetDatabase(settings.DatabaseName);
            Settings = settings;
        }
        
        public MongoDbContext(IMongoClient mongoClient, IMongoDatabase mongoDatabase)
        {
            Client = mongoClient;
            Database = mongoDatabase; 
        }
        
        public IMongoCollection<T> GetCollection<T>() => Database.GetCollection<T>(typeof(T).Name);
        
        // ToDo: Find the right way to remove DB in Tests
        public Task DeleteDatabase()
        {
            return Client.DropDatabaseAsync(Settings.DatabaseName);
        }
    }
}