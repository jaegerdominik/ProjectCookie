using DAL.Entities.Devices;
using DAL.UnitOfWork;

namespace DAL.Repository.Impl
{
    public class ModbusDeviceRepository : EntityRepository<ModbusDevice>, IModbusDeviceRepository
    {
        public ModbusDeviceRepository(TimeScaleContext context) : base(context)
        {

        }

    }
}
