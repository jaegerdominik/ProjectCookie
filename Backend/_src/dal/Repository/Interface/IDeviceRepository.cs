using DAL.Entities;
using ProjectCookie._src.dal.Entities;

namespace DAL.Repository.Impl
{
    public interface IDeviceRepository : IEntityRepository<Device>
    {
        public Task<Device> GetDeviceForAquarium(String aquarium, String device);

        public Task<List<Device>> GetDevicesForAquarium(String aquarium);
    }
}
