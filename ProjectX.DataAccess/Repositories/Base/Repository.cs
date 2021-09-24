using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Repositories.Base
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity<ObjectId>
    {
        protected IMongoContext MongoContext { get; set; }
        private readonly IMongoCollection<TEntity> _mongoCollection;

        protected Repository(IMongoContext mongoContext, string collectionName)
        {
            MongoContext = mongoContext;
            _mongoCollection = MongoContext.Database.GetCollection<TEntity>(collectionName);
        }
        
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var filter = Builders<TEntity>.Filter.Empty;
            var queryResult = await _mongoCollection.FindAsync(filter);
            return queryResult.ToEnumerable();
        }

        public virtual async Task<TEntity> FindById(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, id);

            var queryResult = await _mongoCollection.FindAsync(filter);
            return await queryResult.FirstOrDefaultAsync();
        }

        public virtual Task Insert(TEntity entity) => _mongoCollection.InsertOneAsync(entity);

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(field => entity.Id, entity.Id);
            await _mongoCollection.ReplaceOneAsync(filter, entity);

            return await FindById(entity.Id);
        }

        public virtual async Task<TEntity> Update(ObjectId id, TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, id);
            await _mongoCollection.ReplaceOneAsync(filter, entity);

            return await FindById(entity.Id);
        }

        public virtual Task Delete(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, entity.Id);
            
            return _mongoCollection.DeleteOneAsync(filter);
        }
        
        public virtual Task Delete(ObjectId id)
        {
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, id);
            
            return _mongoCollection.DeleteOneAsync(filter);
        }
    }
}