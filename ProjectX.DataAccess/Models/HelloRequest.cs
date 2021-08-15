using MongoDB.Bson;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Models
{
    public class HelloRequestEntity : Entity<ObjectId>
    {
        public string Message { get; set; }
    }
}