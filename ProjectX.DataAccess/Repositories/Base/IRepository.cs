using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : Entity<ObjectId>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> FindById(ObjectId id);
        Task Insert(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Update(ObjectId id, TEntity entity);
        Task Delete(TEntity entity);
        Task Delete(ObjectId id);
    }
}