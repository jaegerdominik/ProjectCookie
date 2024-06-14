using DAL.Entities.Devices;
using DAL.Repository.Impl;
using ProjectCookie._src.dal.UnitOfWork;

namespace ProjectCookie._src.dal.Repository.Impl
{
    public class MQTTDeviceRepository : EntityRepository<MQTTDevice>, IMQTTDeviceRepository
    {
        public MQTTDeviceRepository(PostgresDbContext context) : base(context)
        {

        }

    }
}
