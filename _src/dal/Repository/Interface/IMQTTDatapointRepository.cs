using DAL.Entities.Devices;

namespace DAL.Repository.Impl
{
    public interface IMQTTDatapointRepository : IEntityRepository<MQTTDataPoint>
    {
        public Task<List<MQTTDataPoint>> GetForDevice(int deviceid);
    }
}
