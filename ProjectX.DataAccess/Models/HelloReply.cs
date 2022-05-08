using MongoDB.Bson;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Models
{
    public class HelloReply : Entity<ObjectId>, IHasOwner<ObjectId>
    {
        public string Message { get; set; }
        
        public ObjectId OwnerId { get; set; }
    }
}