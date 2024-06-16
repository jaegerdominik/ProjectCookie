using System.Linq.Expressions;
using ProjectCookie.DAL.BaseClasses;
using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.UnitOfWork;

namespace ProjectCookie.DAL.Repository
{
    public class PostgresRepository<TEntity> : IPostgresRepository<TEntity> where TEntity : Entity
    {
        protected readonly PostgresDbContext db;


        public PostgresRepository(PostgresDbContext dbContext)
        {
            db = dbContext;
        }


        public IEnumerable<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression)
            => db.Set<TEntity>().Where(filterExpression);

        public async Task<TEntity?> FindByIdAsync(int id)
            => await FindAsync(e => e.ID == id);
        public async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            IEnumerable<TEntity> filtered = FilterBy(filterExpression);
            if (filtered.Any()) return filtered.First();
            return null;
        }

        public async Task InsertAsync(TEntity entity)
        {
            await db.Set<TEntity>().AddAsync(entity);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            db.Set<TEntity>().Update(entity);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            TEntity? entityToRemove = await FindAsync(filterExpression);
            if (entityToRemove == null) return;

            db.Set<TEntity>().Remove(entityToRemove);
            await db.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(int id)
            => await DeleteAsync(e => e.ID == id);
    }
}
