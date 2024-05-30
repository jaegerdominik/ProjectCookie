using dal.MongoDB;
using dal.Repositories.interfaces;

namespace dal.Repositories;

public class UserRepository : MongoRepository<User>, IUserRepository
{
    public UserRepository(DBContext context) : base(context)
    {
    }

    public Task<User> Login(string username, string password)
    {
        var result = FilterBy(u => u.Email == username && u.HashedPassword == password).ToList();
        return Task.FromResult(result.Count > 0 ? result.First() : new User());
        
        //return FindOneAsync(u => u.Email == username && u.Password == password);
    }
}