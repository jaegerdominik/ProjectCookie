using DAL.Entities;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> CreateAsync(TEntity entity);

        Task<List<TEntity>> CreateAsync(List<TEntity> entity);
        Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> Find(Expression<Func<TEntity, bool>> predicate);
        Task Delete(TEntity entity);
        Task Delete(int id);
        Task<TEntity> Update(TEntity entity);

    }
}
