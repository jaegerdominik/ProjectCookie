using System.Linq.Expressions;
using ProjectCookie.DAL.BaseClasses;

namespace ProjectCookie.DAL.BaseInterfaces;

public interface IPostgresRepository<TEntity> where TEntity : Entity
{
    IEnumerable<TEntity> FilterBy(
        Expression<Func<TEntity, bool>> filterExpression);

    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> filterExpression);
    Task<TEntity?> FindByIdAsync(int id);
    Task InsertAsync(TEntity entity);
}