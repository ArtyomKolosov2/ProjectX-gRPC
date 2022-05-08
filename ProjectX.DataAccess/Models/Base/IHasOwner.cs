using MongoDB.Bson;

namespace ProjectX.DataAccess.Models.Base
{
    public interface IHasOwner<T>
    {
        public T OwnerId { get; set; }
    }
}