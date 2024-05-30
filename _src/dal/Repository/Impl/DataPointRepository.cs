using DAL.Entities;
using DAL.UnitOfWork;
using ProjectCookie._src.dal.Repository;
using ProjectCookie._src.dal.UnitOfWork;

namespace DAL.Repository.Impl
{
    public class DataPointRepository : EntityRepository<DataPoint>, IDataPointRepository
    {
        public DataPointRepository(PostgresDbContext context) : base(context)
        {

        }

        public async Task<DataPoint> GetDatapointForDeviceAndAquarium(int dev, string datapoint)
        {

            return await Find(x => x.FK_Device == dev && x.Name.Equals(datapoint));
        }

        public async Task<List<DataPoint>> GetDatapointsForDevice(int dev)
        {
            return await Get(x => x.FK_Device == dev);
        }
    }
}
