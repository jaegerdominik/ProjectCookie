using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.Entities;

namespace ProjectCookie.Services.ServiceImpl;

public class UserService : Service<User>
{
    public UserService(IUserRepository repo) : base(repo) { }

    public override async Task<bool> Validate(User entity)
    {
        if (entity != null)
        {
            if (entity.Username == null)
            {
                ValidationDictionary.AddError("Username missing", "You need to specify a name for your User");
            }
        }
        else
        {
            ValidationDictionary.AddError("Empty", "You did not send a User");
        }

        return ValidationDictionary.IsValid;
    }

    public async Task<User?> GetByName(string username)
    {
        User? user = await Repo.FindAsync((u) => u.Username == username);
        return user;
    }
}