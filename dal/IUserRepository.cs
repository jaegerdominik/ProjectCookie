using dal.interfaces;

namespace dal.Repositories.interfaces;

public interface IUserRepository : IMongoRepository<User>
{
    Task<User> Login(String username, String password);
}