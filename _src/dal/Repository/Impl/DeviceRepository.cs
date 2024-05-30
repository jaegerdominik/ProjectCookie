using DAL.Entities;
using DAL.UnitOfWork;
using ProjectCookie._src.dal.Repository;
using ProjectCookie._src.dal.UnitOfWork;

namespace DAL.Repository.Impl
{
    public class DeviceRepository : EntityRepository<Device>, IDeviceRepository
    {
        public DeviceRepository(PostgresDbContext context) : base(context)
        {

        }

        public async Task<Device> GetDeviceForAquarium(string aquarium, string device)
        {
            return await Find(x => x.Aquarium.Equals(aquarium) && x.DeviceName.Equals(device));
        }

        public async Task<List<Device>> GetDevicesForAquarium(string aquarium)
        {
            return await Get(x => x.Aquarium.Equals(aquarium));
        }
    }
}
