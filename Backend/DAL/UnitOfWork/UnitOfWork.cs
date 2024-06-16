using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.Repository;

namespace ProjectCookie.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly PostgresDbContext _context;

    public UnitOfWork(PostgresDbContext context)
    {
        _context = context;
    }


    public IUserRepository Users => new UserRepository(_context);
    public IScoreRepository Scores => new ScoreRepository(_context);

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
}