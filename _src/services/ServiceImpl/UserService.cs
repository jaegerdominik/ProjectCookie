using dal;
using dal.interfaces;
using dal.Repositories;
using services.Authentication;
using services.interfaces;
using utils.interfaces;

namespace services.ServiceImpl;

public class UserService : Service<User>
{
    public UserService(IUnitOfWork unitOfWork, IMongoRepository<User> repo, IAquariumLogger logger, IAuthenticator auth) : base(unitOfWork, repo, logger)
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

    public async Task<AuthenticationInformation> Login(string username, string password)
    {
        // user suchen
        User user = await UnitOfWork.User.Login(username, password);
        
        // user validieren
        bool isValidUser = await Validate(user);
        if (!isValidUser)
        {
            return null;
        }
            
        // wenn er validiert, wird er authentifiziert
        Authenticator authenticator = new(UnitOfWork);
        AuthenticationInformation info = await authenticator.Authenticate(user);
        if (info.Token == null)
        {
            return null;
        }

        return info;
    }
}