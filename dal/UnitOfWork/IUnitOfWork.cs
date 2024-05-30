using DAL.Entities;
using DAL.Repository;
using DAL.Repository.Impl;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        TimeScaleContext Context { get; }

        IDataPointRepository DataPoints { get; }

        IDeviceRepository Devices { get; }

        IMQTTDeviceRepository MQTTDevices { get; }

        IModbusDeviceRepository ModbusDevices { get; }

        IMQTTDatapointRepository MQTTDatapoint { get; }

        IModbusDatapointRepository ModbusDatapoint { get; }

        IHypertableRepository<NumericSample> Numeric { get; }

        IHypertableRepository<BinarySample> Binary { get; }
        Task<int> SaveChangesAsync();


    }
}
