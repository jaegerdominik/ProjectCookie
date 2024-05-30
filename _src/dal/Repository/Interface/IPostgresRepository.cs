using System.Linq.Expressions;
using DAL.Entities;

namespace ProjectCookie._src.dal.Repository.Interface
{
    public interface IPostgresRepository<TEntity> where TEntity : Entity
    {
        IEnumerable<TEntity> FilterBy(
            Expression<Func<TEntity, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TEntity, bool>> filterExpression,
            Expression<Func<TEntity, TProjected>> projectionExpression);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filterExpression);
        Task<TEntity> FindByIdAsync(int id);
        Task InsertAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(Expression<Func<TEntity, bool>> filterExpression);
        Task DeleteByIdAsync(int id);
    }
}
