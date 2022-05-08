using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using ProjectX.DataAccess.Context.Base;
using ProjectX.DataAccess.Models.Base;
using static ProjectX.Core.ArgumentRule.ArgumentRule;

namespace ProjectX.DataAccess.Repositories.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity<ObjectId>
    {
        protected IMongoContext MongoContext { get; set; }
        protected IMongoCollection<TEntity> MongoCollection { get; set; }

        public Repository(IMongoContext mongoContext)
        {
            MongoContext = mongoContext;
            MongoCollection = MongoContext.GetCollection<TEntity>();
        }
        
        public virtual async Task<IEnumerable<TEntity>> GetAll()
        {
            var filter = Builders<TEntity>.Filter.Empty;
            var queryResult = await MongoCollection.FindAsync(filter);
            return queryResult.ToEnumerable();
        }

        public virtual async Task<TEntity> FindById(ObjectId id)
        {
            Requires(id != ObjectId.Empty, "Id should not be empty!");
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, id);

            var queryResult = await MongoCollection.FindAsync(filter);
            return await queryResult.FirstOrDefaultAsync();
        }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            NotNull(entity, nameof(entity));
            
            await MongoCollection.InsertOneAsync(entity);
            return entity;
        }

        public virtual async Task<TDerived> Insert<TDerived>(TEntity entity) where TDerived : TEntity
        {
            NotNull(entity, nameof(entity));
            Requires(entity is TDerived, $"{entity.GetType().Name} is not assignable to {typeof(TDerived).Name}");
            
            await MongoCollection.InsertOneAsync(entity);
            return (TDerived)entity;
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            NotNull(entity, nameof(entity));
            var filter = Builders<TEntity>.Filter.Eq(field => entity.Id, entity.Id);
            await MongoCollection.ReplaceOneAsync(filter, entity);

            return await FindById(entity.Id);
        }

        public virtual async Task<TEntity> Update(ObjectId id, TEntity entity)
        {
            NotNull(entity, nameof(entity));
            Requires(id != ObjectId.Empty, "Id should not be empty!");
            
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, id);
            await MongoCollection.ReplaceOneAsync(filter, entity);

            return await FindById(entity.Id);
        }

        public virtual Task Delete(TEntity entity)
        {
            NotNull(entity, nameof(entity));
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, entity.Id);
            
            return MongoCollection.DeleteOneAsync(filter);
        }
        
        public virtual Task Delete(ObjectId id)
        {
            Requires(id != ObjectId.Empty, "Id should not be empty!");
            var filter = Builders<TEntity>.Filter.Eq(field => field.Id, id);
            
            return MongoCollection.DeleteOneAsync(filter);
        }
        
        public IQueryable<TEntity> AsQueryable() => MongoCollection.AsQueryable();
    }
}