using dal.interfaces;
using services.interfaces;
using services.ServiceImpl;
using utils.interfaces;

namespace api;

public class GlobalService : IGlobalService
{
    public AquariumService AquariumService { get; set; }
    public UserService UserService { get; set; }
    public CoralService CoralService { get; set; }
    public AnimalService AnimalService { get; set; }
    public IUnitOfWork UnitOfWork { get; set; }


    public GlobalService(IUnitOfWork unitOfWork, IAquariumLogger logger, IAuthenticator auth)
    {
        AquariumService = new AquariumService(unitOfWork,unitOfWork.Aquarium, logger);
        UserService = new UserService(unitOfWork, unitOfWork.User, logger, auth);
        CoralService = new CoralService(unitOfWork,unitOfWork.AquariumItem, logger);
        AnimalService = new AnimalService(unitOfWork, unitOfWork.AquariumItem, logger);

        UnitOfWork = unitOfWork;

    }
}