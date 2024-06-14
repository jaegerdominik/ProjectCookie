using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.Entities;
using ProjectCookie.Utils.Logging;

namespace ProjectCookie.Services.ServiceImpl;

public class UserService : Service<User>
{
    public UserService(ICookieLogger logger, IUnitOfWork unitOfWork, IPostgresRepository<User> repo)
        : base(unitOfWork, repo, logger) { }

    
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
}