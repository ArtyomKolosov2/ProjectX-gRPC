using MongoDB.Bson;

namespace ProjectX.DataAccess.Models.Base
{
    public class BusinessUser : Entity<ObjectId>
    {
        public ObjectId UserId { get; init; }
        
        public string Email { get; set; }
    }
}