using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using ProjectX.DataAccess.Models.Base;

namespace ProjectX.DataAccess.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : Entity<ObjectId>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> FindById(ObjectId id);
        Task<TEntity> Insert(TEntity entity);
        Task<TDerived> Insert<TDerived>(TEntity entity) where TDerived : TEntity;
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Update(ObjectId id, TEntity entity);
        Task Delete(TEntity entity);
        Task Delete(ObjectId id);
        IQueryable<TEntity> AsQueryable();
    }
}