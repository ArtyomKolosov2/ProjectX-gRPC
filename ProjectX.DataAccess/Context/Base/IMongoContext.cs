using System.Threading.Tasks;
using MongoDB.Driver;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Context.Base
{
    public interface IMongoContext
    { 
        IMongoClient Client { get; set; }
        IMongoDatabase Database { get; set; }
        IDatabaseSettings Settings { get; set; }

        Task DeleteDatabase();
    }
}