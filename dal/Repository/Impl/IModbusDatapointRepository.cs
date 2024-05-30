using DAL.Entities.Devices;

namespace DAL.Repository.Impl
{
    public interface IModbusDatapointRepository : IEntityRepository<ModbusDataPoint>
    {
        public Task<List<ModbusDataPoint>> GetForDevice(int deviceid);
    }
}
