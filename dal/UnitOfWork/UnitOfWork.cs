using DAL.Entities;
using DAL.Repository;
using DAL.Repository.Impl;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public TimeScaleContext Context { get; private set; } = null;
        public UnitOfWork(TimeScaleContext context)
        {
            Context = context;
        }

        public IDataPointRepository DataPoints
        {
            get
            {
                return new DataPointRepository(Context);
            }
        }

        public IDeviceRepository Devices
        {
            get
            {
                return new DeviceRepository(Context);
            }
        }

        public IMQTTDeviceRepository MQTTDevices
        {
            get
            {
                return new MQTTDeviceRepository(Context);
            }
        }

        public IModbusDeviceRepository ModbusDevices
        {
            get
            {
                return new ModbusDeviceRepository(Context);
            }
        }



        public IMQTTDatapointRepository MQTTDatapoint
        {
            get
            {
                return new MQTTDatapointRepository(Context);
            }
        }

        public IModbusDatapointRepository ModbusDatapoint
        {
            get
            {
                return new ModbusDatapointRepository(Context);
            }
        }



        public IHypertableRepository<NumericSample> Numeric
        {
            get
            {
                return new HypertableRepository<NumericSample>(Context);
            }
        }

        public IHypertableRepository<BinarySample> Binary
        {
            get
            {
                return new HypertableRepository<BinarySample>(Context);
            }
        }


        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
        

    }
}
