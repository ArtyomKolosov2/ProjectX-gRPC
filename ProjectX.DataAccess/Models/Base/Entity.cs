using MongoDB.Bson.Serialization.Attributes;

namespace ProjectX.DataAccess.Models.Base
{
    public abstract class Entity<T>
    {
        [BsonId]
        public T Id { get; set; }
    }
}