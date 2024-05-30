using DAL.Entities.Devices;
using DAL.UnitOfWork;
using ProjectCookie._src.dal.Repository;
using ProjectCookie._src.dal.UnitOfWork;

namespace DAL.Repository.Impl
{
    public class MQTTDeviceRepository : EntityRepository<MQTTDevice>, IMQTTDeviceRepository
    {
        public MQTTDeviceRepository(PostgresDbContext context) : base(context)
        {

        }

    }
}
