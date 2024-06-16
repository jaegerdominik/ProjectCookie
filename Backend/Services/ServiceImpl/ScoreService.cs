using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.Entities;
using ProjectCookie.Utils.Logging;

namespace ProjectCookie.Services.ServiceImpl;

public class ScoreService : Service<Score>
{
    public ScoreService(IScoreRepository repo) : base(repo) { }

    
    public override async Task<bool> Validate(Score entity)
    {
        if (entity != null)
        {
            if (entity.Points == null)
            {
                ValidationDictionary.AddError("Points missing", "You need points.");
            }
        }
        else
        {
            ValidationDictionary.AddError("Empty", "You did not send a User");
        }

        return ValidationDictionary.IsValid;
    }
}