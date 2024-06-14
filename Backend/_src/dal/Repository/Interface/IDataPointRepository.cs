using DAL.Repository;
using ProjectCookie._src.dal.Entities;

namespace ProjectCookie._src.dal.Repository.Interface
{
    public interface IDataPointRepository : IEntityRepository<DataPoint>
    {
        public Task<DataPoint> GetDatapointForDeviceAndAquarium(int dev, String datapoint);

        public Task<List<DataPoint>> GetDatapointsForDevice(int dev);
    }
}
