﻿using MongoDB.Driver;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Context
{
    public class MongoDbContext : IMongoContext
    {
        public IMongoClient Client { get; set; }
        public IMongoDatabase Database { get; set; }

        public MongoDbContext(IDatabaseSettings settings)
        {
            Client = new MongoClient(settings.ConnectionString);
            Database = Client.GetDatabase(settings.DatabaseName);
        }
        
        public MongoDbContext(IMongoClient mongoClient, IMongoDatabase mongoDatabase)
        {
            Client = mongoClient;
            Database = mongoDatabase; 
        }
    }
}