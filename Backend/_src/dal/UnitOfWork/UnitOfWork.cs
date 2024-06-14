using DAL.Repository.Impl;
using DAL.UnitOfWork;
using ProjectCookie._src.dal.Repository.Impl;
using ProjectCookie._src.dal.Repository.Interface;

namespace ProjectCookie._src.dal.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public PostgresDbContext Context { get; private set; } = null;
        public IUserRepository Users { get; }

        public UnitOfWork(PostgresDbContext context)
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
        
        public IMQTTDatapointRepository MQTTDatapoint
        {
            get
            {
                return new MQTTDatapointRepository(Context);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }
        

    }
}
