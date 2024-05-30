using DAL.Entities;
using DAL.Repository;
using DAL.Repository.Impl;
using ProjectCookie._src.dal;
using ProjectCookie._src.dal.UnitOfWork;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        PostgresDbContext Context { get; }
        IUserRepository Users { get; }
        
        IDataPointRepository DataPoints { get; }

        IDeviceRepository Devices { get; }

        IMQTTDeviceRepository MQTTDevices { get; }
        
        IMQTTDatapointRepository MQTTDatapoint { get; }
        
        Task<int> SaveChangesAsync();


    }
}
