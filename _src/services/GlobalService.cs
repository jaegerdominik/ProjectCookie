using DAL.UnitOfWork;
using Utilities.Logging;

namespace Services
{
    public class GlobalService : IGlobalService
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public DeviceService DeviceService { get; set; }
        public ModbusDeviceService ModbusDeviceService { get; set; }
        public MQTTDeviceService MQTTDeviceService { get; set; }

        public ValueService ValueService { get; set; }

        public GlobalService(IUnitOfWork UnitOfWork, IAquariumLogger Logger)
        {
            this.UnitOfWork = UnitOfWork;


            DeviceService = new DeviceService(Logger, UnitOfWork, UnitOfWork.Devices, this);
            ModbusDeviceService = new ModbusDeviceService(Logger, UnitOfWork, UnitOfWork.Devices, this);
            MQTTDeviceService = new MQTTDeviceService(Logger, UnitOfWork, UnitOfWork.Devices, this);
            ValueService = new ValueService(UnitOfWork);

        }
    }
}
