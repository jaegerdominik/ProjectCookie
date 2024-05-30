using DAL.UnitOfWork;

namespace Services
{
    public interface IGlobalService
    {
        public IUnitOfWork UnitOfWork { get; set; }

        public DeviceService DeviceService { get; set; }
        public ModbusDeviceService ModbusDeviceService { get; set; }
        public MQTTDeviceService MQTTDeviceService { get; set; }

        public ValueService ValueService { get; set; }
    }
}
