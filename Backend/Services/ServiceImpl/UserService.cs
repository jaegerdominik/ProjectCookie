using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.Entities;
using Serilog;

namespace ProjectCookie.Services.ServiceImpl;

public class UserService : Service<User>
{
    public UserService(IUserRepository repo) : base(repo) { }

    public override async Task<bool> Validate(User entity)
    {
        Log.Information("INSIDE VALIDATE with param: " + entity);

        if (entity != null)
        {
            Log.Information("entity is not null.");
            if (entity.Username == null)
            {
                ValidationDictionary.AddError("Username missing", "You need to specify a name for your User");
            }
            Log.Information("entity username is not missing.");
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