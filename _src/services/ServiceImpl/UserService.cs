using DAL.UnitOfWork;
using ProjectCookie._src.dal;
using ProjectCookie._src.dal.Repository.Interface;
using ProjectCookie._src.utils.Logging;

namespace ProjectCookie._src.services.ServiceImpl;

public class UserService : Service<User>
{
    public UserService(ICookieLogger logger, IUnitOfWork unitOfWork, IPostgresRepository<User> repo) : base(unitOfWork, repo, logger)
    {
    }

    public override async Task<bool> Validate(User entity)
    {
        if (entity != null)
        {
            if (entity.Email == null)
            {
                ValidationDictionary.AddError("Email missing", "You need to specify an email for your User");
            }
            if (entity.Firstname == null)
            {
                ValidationDictionary.AddError("Firstname missing", "You need to specify a firstname for your User");    
            }
            if (entity.Lastname == null)
            {
                ValidationDictionary.AddError("Lastname missing", "You need to specify a lastname for your User");
            }
            if (entity.Active == null)
            {
                ValidationDictionary.AddError("Active parameter missing", "You need to specify if the User is active");
            }
        }
        else
        {
            ValidationDictionary.AddError("Empty", "You did not send a User");
        }

        return ValidationDictionary.IsValid;
    }
}