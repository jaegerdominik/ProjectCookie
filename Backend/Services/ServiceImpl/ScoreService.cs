using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.Entities;
using ProjectCookie.Utils.Logging;

namespace ProjectCookie.Services.ServiceImpl;

public class ScoreService : Service<Score>
{
    public ScoreService(ICookieLogger logger, IUnitOfWork unitOfWork, IPostgresRepository<Score> repo)
        : base(unitOfWork, repo, logger) { }

    
    public override async Task<bool> Validate(Score entity)
    {
        if (entity != null)
        {
            if (entity.Points == null)
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