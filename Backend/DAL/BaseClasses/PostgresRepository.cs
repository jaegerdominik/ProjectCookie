using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.UnitOfWork;

namespace ProjectCookie.DAL.BaseClasses
{
    public class PostgresRepository<TEntity> : IPostgresRepository<TEntity> where TEntity : Entity
    {
        protected readonly PostgresDbContext _db;
        protected readonly DbSet<TEntity> _dbSet;

        public PostgresRepository(PostgresDbContext dbContext)
        {
            _db = dbContext;
            _dbSet = _db.Set<TEntity>();
        }


        public IEnumerable<TEntity> FilterBy(Expression<Func<TEntity, bool>> filterExpression)
            => _dbSet.Where(filterExpression);

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
            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();
        }
    }
}
