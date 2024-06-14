using DAL.UnitOfWork;
using ProjectCookie._src.services.ServiceImpl;
using Services;

namespace ProjectCookie._src.services.interfaces
{
    public interface IGlobalService
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public DeviceService DeviceService { get; set; }
        public MQTTDeviceService MQTTDeviceService { get; set; }

        public ValueService ValueService { get; set; }
        public UserService UserService { get; set; }
    }
}
