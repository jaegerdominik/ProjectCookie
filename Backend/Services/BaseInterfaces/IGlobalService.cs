using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.Services.ServiceImpl;

namespace ProjectCookie.Services.BaseInterfaces;

public interface IGlobalService
{
    public IUnitOfWork UnitOfWork { get; set; }

    public UserService UserService { get; set; }
    public ScoreService ScoreService { get; set;  }
}