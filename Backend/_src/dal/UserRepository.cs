using ProjectCookie._src.dal.Repository.Impl;
using ProjectCookie._src.dal.UnitOfWork;

namespace ProjectCookie._src.dal;

public class UserRepository : PostgresRepository<User>, IUserRepository
{
    public UserRepository(PostgresDbContext context) : base(context)
    {
    }

    public Task<User> Login(string username, string password)
    {
        var result = FilterBy(u => u.Email == username && u.Password == password).ToList();
        return Task.FromResult(result.Count > 0 ? result.First() : new User());
        
        //return FindOneAsync(u => u.Email == username && u.Password == password);
    }
}