using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Repositories.Base
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity<ObjectId>
    {
        protected IMongoContext MongoContext { get; set; }
        protected IMongoCollection<TEntity> MongoCollection { get; set; }

        protected Repository(IMongoContext mongoContext, string collectionName)
        {
            MongoContext = mongoContext;
            MongoCollection = MongoContext.Database.GetCollection<TEntity>(collectionName);
        }
        
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var filter = Builders<TEntity>.Filter.Empty;
            var queryResult = await MongoCollection.FindAsync(filter);
            return queryResult.ToEnumerable();
        }

        public virtual async Task<TEntity> FindById(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, id);

            var queryResult = await MongoCollection.FindAsync(filter);
            return await queryResult.FirstOrDefaultAsync();
        }

        public virtual Task Insert(TEntity entity) => MongoCollection.InsertOneAsync(entity);

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(field => entity.Id, entity.Id);
            await MongoCollection.ReplaceOneAsync(filter, entity);

            return await FindById(entity.Id);
        }

        public virtual async Task<TEntity> Update(ObjectId id, TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, id);
            await MongoCollection.ReplaceOneAsync(filter, entity);

            return await FindById(entity.Id);
        }

        public virtual Task Delete(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, entity.Id);
            
            return MongoCollection.DeleteOneAsync(filter);
        }
        
        public virtual Task Delete(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, id);
            
            return MongoCollection.DeleteOneAsync(filter);
        }
        
        public IMongoQueryable AsQueryable() => MongoCollection.AsQueryable();
    }
}