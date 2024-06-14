namespace ProjectCookie.DAL.BaseInterfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IScoreRepository Scores { get; }
        
    Task<int> SaveChangesAsync();
}