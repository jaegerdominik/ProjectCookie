using DAL.Entities.Devices;
using DAL.UnitOfWork;
using ProjectCookie._src.dal.Repository;
using ProjectCookie._src.dal.UnitOfWork;

namespace DAL.Repository.Impl
{
    public class MQTTDatapointRepository : EntityRepository<MQTTDataPoint>, IMQTTDatapointRepository
    {
        public MQTTDatapointRepository(PostgresDbContext context) : base(context)
        {

        }

        public async Task<List<MQTTDataPoint>> GetForDevice(int deviceid)
        {
            return await Get(x => x.FK_Device == deviceid);
        }
    }
}
