using DAL.Entities;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class HypertableRepository<T>(TimeScaleContext dbContext) : IHypertableRepository<T>
    where T : HyperEntity
{
    private readonly TimeScaleContext _dbContext = dbContext;
    private readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task CreateAsync(List<T> entities)
    {
        foreach (var entity in entities)
        {
            await CreateAsync(entity);
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task<T> GetLastValue(int dp)
    {
        return _dbSet.OrderBy(v => v.CreationDate).Last(t => t.FK_Datapoint == dp);
    }

    public async Task<T> GetLastValue(int dp, DateTimeOffset from, DateTimeOffset to)
    {
        return _dbSet.OrderBy(v => v.CreationDate).Last(t => t.FK_Datapoint == dp && t.Time >= from && t.Time <= to);
    }

    public async Task<List<T>> GetValues(int dp, DateTimeOffset from, DateTimeOffset to)
    {
        return _dbSet.Where(t => t.FK_Datapoint == dp && t.Time >= from && t.Time <= to).ToList();
    }
}