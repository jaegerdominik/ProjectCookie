using DAL.Entities;
using DAL.Entities.Devices;
using DAL.Repository.Impl;
using DAL.UnitOfWork;
using Services.Response.Basis;
using Utilities.Logging;

namespace Services
{
    public class MQTTDeviceService : DeviceService
    {
        public MQTTDeviceService(IAquariumLogger logger, IUnitOfWork uow, IDeviceRepository repo, IGlobalService service) : base(logger, uow, repo, service)
        {
        }

        public override async Task<bool> Validate(Device entry)
        {
            if (entry.GetType() == typeof(MQTTDevice))
            {
                Boolean result = await base.Validate(entry);

                if (!result)
                {
                    return false;
                }
                else
                {
                    MQTTDevice device = (MQTTDevice)entry;
                    if (!String.IsNullOrEmpty(device.Host))
                    {
                        validationDictionary.AddError("NotValid", "Host is not valid");
                    }

                    if (device.Port <= 0 | device.Port >= 65536)
                    {
                        validationDictionary.AddError("NotValid", "Port is not valid");
                    }


                }
            }
            else
            {
                validationDictionary.AddError("NotValid", "Item is no MQTT Device");
            }

            return validationDictionary.IsValid;
        }


        public async Task<ItemResponseModel<MQTTDevice>> AddMQTTDevice(MQTTDevice entry)
        {
            ItemResponseModel<MQTTDevice> coralresp = new ItemResponseModel<MQTTDevice>();
            ItemResponseModel<Device> resp = await base.Create(entry);

            if (resp.HasError == false)
            {
                coralresp.Data = resp.Data as MQTTDevice;
            }
            else
            {
                coralresp.AddErrorMessageRange(resp.ErrorMessages);
                coralresp.AddWarningMessageRange(resp.WarningMessages);
                coralresp.HasError = true;
            }
            return coralresp;
        }
    }
}
