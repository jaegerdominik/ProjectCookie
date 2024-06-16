using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.Repository;

namespace ProjectCookie.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    public PostgresDbContext Context;

    public UnitOfWork(PostgresDbContext context)
    {
        Context = context;
    }


    public IUserRepository Users => new UserRepository(Context);
    public IScoreRepository Scores => new ScoreRepository(Context);

    public async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();
}