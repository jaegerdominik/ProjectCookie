using DAL.Entities;

namespace DAL.Repository.Impl
{
    public interface IDataPointRepository : IEntityRepository<DataPoint>
    {
        public Task<DataPoint> GetDatapointForDeviceAndAquarium(int dev, String datapoint);

        public Task<List<DataPoint>> GetDatapointsForDevice(int dev);
    }
}
