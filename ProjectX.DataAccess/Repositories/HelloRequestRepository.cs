using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models;
using ProjectX.DataAccess.Repositories.Base;

namespace ProjectX.DataAccess.Repositories
{
    public class HelloRequestRepository : Repository<HelloRequestEntity>
    {
        private const string CollectionName = "HelloRequests";
        
        public HelloRequestRepository(IMongoContext mongoContext) : base(mongoContext, CollectionName)
        {
            
        }
    }
}