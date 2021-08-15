using MongoDB.Driver;

namespace ProjectX.DataAccess.Context.Base
{
    public interface IMongoContext
    { 
        IMongoClient Client { get; set; }
        IMongoDatabase Database { get; set; }
    }
}