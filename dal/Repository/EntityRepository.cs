using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository;

public class EntityRepository<TEntity>(TimeScaleContext dbContext) : IEntityRepository<TEntity>
    where TEntity : Entity
{
    private readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<List<TEntity>> CreateAsync(List<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            await CreateAsync(entity);
        }

        await dbContext.SaveChangesAsync();
        return entities;
    }

    public async Task Delete(TEntity entity)
    {
        var e = await _dbSet.FindAsync(entity.ID);
        if (e != null)
        {
            _dbSet.Remove(entity);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task Delete(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity != null)
        {
            await Delete(entity);
            await dbContext.SaveChangesAsync();
        }
    }

    public Task<TEntity?> Find(Expression<Func<TEntity, bool>> predicate)
    {
        return Task.FromResult(_dbSet.FirstOrDefault(predicate));
    }

    public async Task<List<TEntity>> Get(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.AsQueryable().Where(predicate).ToList();
    }

    public async Task<TEntity> Update(TEntity entity)
    {
        _dbSet.Update(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }
}