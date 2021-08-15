using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Models
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}