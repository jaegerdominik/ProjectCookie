using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.Entities;
using ProjectCookie.DAL.UnitOfWork;

namespace ProjectCookie.DAL.Repository;

public class UserRepository : PostgresRepository<User>, IUserRepository
{
    public UserRepository(PostgresDbContext context) : base(context) { }
}