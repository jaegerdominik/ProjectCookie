using dal.interfaces;
using services.ServiceImpl;

namespace api;

public interface IGlobalService
{
    public AquariumService AquariumService { get; set; }
    public UserService UserService { get; set; }
    public CoralService CoralService { get; set; }

    //public PictureService PictureService { get; set; }
    public AnimalService AnimalService { get; set; }
    public IUnitOfWork UnitOfWork { get; set; }
}