using DAL.Entities;
using DAL.Repository.Impl;
using DAL.UnitOfWork;
using ProjectCookie._src.dal.Entities;
using ProjectCookie._src.services.interfaces;
using ProjectCookie._src.utils.Logging;
using Services.Response.Basis;
using Utilities.Logging;

namespace Services
{
    public class DeviceService : DataService<Device>
    {
        IDeviceRepository Repository;

        public DeviceService(ICookieLogger Logger, IUnitOfWork uow, IDeviceRepository repo, IGlobalService service) : base(Logger)
        {
            Repository = repo;
        }

        public override async Task<ItemResponseModel<Device>> Update(int id, Device entry)
        {
            ItemResponseModel<Device> ret = new ItemResponseModel<Device>();
            ret.HasError = true;
            Device anf = await UnitOfWork.Devices.Find(x => x.ID == id);

            if (anf != null)
            {
                entry.ID = id;
                ret.Data = entry;
                ret.HasError = false;
            }
            return ret;
        }

        public override async Task<bool> Validate(Device entry)
        {
            if (entry != null)
            {
                if (String.IsNullOrEmpty(entry.Aquarium))
                {
                    validationDictionary.AddError("AquariumMissing", "No Aquarium was set");
                }
                if (String.IsNullOrEmpty(entry.DeviceName))
                {
                    validationDictionary.AddError("DeviceNameMissing", "No Device Name was set");
                }
            }
            else
            {
                validationDictionary.AddError("ItemEmpty", "Object is empty");
            }

            return validationDictionary.IsValid;

        }

        protected override async Task<ItemResponseModel<Device>> Create(Device entry)
        {
            ItemResponseModel<Device> ret = new ItemResponseModel<Device>();
            Device data = await Repository.CreateAsync(entry);

            ret.Data = data;
            ret.HasError = false;
            return ret;
        }



        public async Task<List<Device>> GetAll(String aquarium)
        {
            List<Device> ent = await Repository.Get(x => x.Aquarium.Equals(aquarium));

            return ent;
        }




    }
}
