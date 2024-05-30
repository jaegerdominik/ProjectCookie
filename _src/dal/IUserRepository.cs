using ProjectCookie._src.dal.Repository.Interface;

namespace ProjectCookie._src.dal;

public interface IUserRepository : IPostgresRepository<User>
{
    Task<User> Login(String username, String password);
}