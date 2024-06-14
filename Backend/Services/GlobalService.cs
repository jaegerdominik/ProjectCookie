using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.Services.BaseInterfaces;
using ProjectCookie.Services.ServiceImpl;
using ProjectCookie.Utils.Logging;

namespace ProjectCookie.Services;

public class GlobalService : IGlobalService
{
    public IUnitOfWork UnitOfWork { get; set; }
    public UserService UserService { get; set; }
    public ScoreService ScoreService { get; set; }

    
    public GlobalService(IUnitOfWork uow, ICookieLogger Logger)
    {
        UnitOfWork = uow;

        UserService = new UserService(Logger, UnitOfWork, UnitOfWork.Users);
        ScoreService = new ScoreService(Logger, UnitOfWork, UnitOfWork.Scores);
    }
}