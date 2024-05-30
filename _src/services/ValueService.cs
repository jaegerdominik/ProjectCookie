using DAL.Entities;
using DAL.UnitOfWork;
using DataCollector.ReturnModels;

namespace Services
{
    public class ValueService
    {
        IUnitOfWork UnitOfWork = null;

        public ValueService(IUnitOfWork UnitOfWork)
        {
            this.UnitOfWork = UnitOfWork;
        }

        public async Task<ValueReturnModelSingle> GetLastValue(string aquarium, string device, string datapoint)
        {
            var dev = await UnitOfWork.Devices.GetDeviceForAquarium(aquarium, device);
            if (dev == null) return null;

            var dp = await UnitOfWork.DataPoints.GetDatapointForDeviceAndAquarium(dev.ID, datapoint);
            if (dp == null) return null;

            ValueReturnModelSingle returnVal = null;

            try
            {
                if (dp.DataType == DataType.Boolean)
                {
                    var binModel = new ValueReturnBinaryModel
                    {
                        Sample = await UnitOfWork.Binary.GetLastValue(dp.ID),
                        TextForTrue = "On",
                        TextForFalse = "Off"
                    };

                    returnVal = binModel;
                }
                else
                {
                    var numModel = new ValueReturnNumericModel
                    {
                        Sample = await UnitOfWork.Numeric.GetLastValue(dp.ID),
                        Unit = dp.Unit,
                        Icon = dp.Icon,
                    };

                    returnVal = numModel;
                }

                returnVal.DataType = dp.DataType;
                returnVal.DataPoint = dp.Name;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return returnVal;
        }

        public async Task<ValueReturnModelMultiple> GetLastValues(string aquarium, string device)
        {
            var dev = await UnitOfWork.Devices.GetDeviceForAquarium(aquarium, device);
            if (dev == null) return null;

            ValueReturnModelMultiple returnVal = new();

            var dps = await UnitOfWork.DataPoints.GetDatapointsForDevice(dev.ID);
            foreach (var val in dps.Select(dp => GetLastValue(aquarium, device, dp.Name).Result))
            {
                if (val == null) continue;
                if (val.DataType == DataType.Boolean)
                {
                    returnVal.Binary.Add((ValueReturnBinaryModel)val);
                }
                else
                {
                    returnVal.Numeric.Add((ValueReturnNumericModel)val);
                }
            }

            return returnVal;
        }

        public async Task<ValueReturnModelMultiple> GetLastValues(string aquarium)
        {
            var devs = await UnitOfWork.Devices.GetDevicesForAquarium(aquarium);
            if (devs == null) return null;

            ValueReturnModelMultiple returnVal = new();

            foreach (var result in devs.Select(dev => GetLastValues(aquarium, dev.DeviceName).Result))
            {
                returnVal.Binary.AddRange(result.Binary);
                returnVal.Numeric.AddRange(result.Numeric);
            }

            return returnVal;
        }
    }
}
