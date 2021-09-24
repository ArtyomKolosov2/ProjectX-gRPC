using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Models.Files
{
    public class FileRecord : Entity<ObjectId>
    {
        [BsonRequired]
        public ObjectId GridFsFileId { get; set; }
        
        [BsonRequired]
        public string Name { get; set; }
        
        [BsonRequired]
        public string Extension { get; set; }
        
        public int SizeInKb { get; set; }
    }
}