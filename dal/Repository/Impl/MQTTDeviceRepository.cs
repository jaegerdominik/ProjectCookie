using DAL.Entities.Devices;
using DAL.UnitOfWork;

namespace DAL.Repository.Impl
{
    public class MQTTDeviceRepository : EntityRepository<MQTTDevice>, IMQTTDeviceRepository
    {
        public MQTTDeviceRepository(TimeScaleContext context) : base(context)
        {

        }

    }
}
