using DAL.Repository.Impl;
using ProjectCookie._src.dal.Entities;
using ProjectCookie._src.dal.UnitOfWork;

namespace ProjectCookie._src.dal.Repository.Impl
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
