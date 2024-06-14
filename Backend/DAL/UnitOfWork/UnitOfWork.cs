using ProjectCookie.DAL.BaseInterfaces;

namespace ProjectCookie.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public PostgresDbContext Context { get; private set; } = null;
        public IUserRepository Users { get; }
        public IScoreRepository Scores { get; }

        public UnitOfWork(PostgresDbContext context)
        {
            Context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
    }
}
