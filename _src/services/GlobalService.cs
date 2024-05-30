using DAL.UnitOfWork;
using ProjectCookie._src.services.interfaces;
using ProjectCookie._src.services.ServiceImpl;
using ProjectCookie._src.utils.Logging;
using Utilities.Logging;

namespace Services
{
    public class GlobalService : IGlobalService
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public DeviceService DeviceService { get; set; }
        
        public UserService UserService { get; set; }

        public MQTTDeviceService MQTTDeviceService { get; set; }

        public ValueService ValueService { get; set; }

        public GlobalService(IUnitOfWork UnitOfWork, ICookieLogger Logger)
        {
            this.UnitOfWork = UnitOfWork;

            UserService = new UserService(Logger, UnitOfWork, UnitOfWork.Users);
            DeviceService = new DeviceService(Logger, UnitOfWork, UnitOfWork.Devices, this);
            MQTTDeviceService = new MQTTDeviceService(Logger, UnitOfWork, UnitOfWork.Devices, this);
            ValueService = new ValueService(UnitOfWork);

        }
    }
}
