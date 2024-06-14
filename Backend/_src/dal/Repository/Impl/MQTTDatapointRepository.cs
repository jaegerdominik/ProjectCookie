using DAL.Entities.Devices;
using DAL.Repository.Impl;
using ProjectCookie._src.dal.UnitOfWork;

namespace ProjectCookie._src.dal.Repository.Impl
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
