using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models.Files;
using ProjectX.DataAccess.Repositories.Base;

namespace ProjectX.DataAccess.Repositories.Files
{
    public class FileRecordRepository : Repository<FileRecord>
    {
        private const string CollectionName = "FileRecords";
        
        public FileRecordRepository(IMongoContext mongoContext) : base(mongoContext, CollectionName)
        {
        }
    }
}