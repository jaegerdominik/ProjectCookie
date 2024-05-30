using DAL.Entities.Devices;
using DAL.UnitOfWork;

namespace DAL.Repository.Impl
{
    public class ModbusDatapointRepository : EntityRepository<ModbusDataPoint>, IModbusDatapointRepository
    {
        public ModbusDatapointRepository(TimeScaleContext context) : base(context)
        {

        }

        public async Task<List<ModbusDataPoint>> GetForDevice(int deviceid)
        {
            return await Get(x => x.FK_Device == deviceid);
        }
    }
}
