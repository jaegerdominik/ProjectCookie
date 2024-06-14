using DAL.Entities;
using DAL.Entities.Devices;
using NUnit.Framework;
using ProjectCookie._src.dal.Entities;
using Tests;

namespace ProjectCookie._src.tests
{
    public class CreateDevicesTest : BaseUnitTest
    {
        private static MQTTDevice TestMqttDevice
            => new()
            {
                Active = true,
                Aquarium = "SchiScho2",
                DeviceName = "WaterQuality",
                DeviceDescription = "Water Quality Measurement",
                Host = "127.0.0.1",
                Port = 1883
            };


        [Test]
        public async Task CreateDevice()
        {
            Device testMqttDevice = await UnitOfWork.Devices.Find(d => d.DeviceName == "WaterQuality");
            if (testMqttDevice == null) await UnitOfWork.Devices.CreateAsync(TestMqttDevice);
        }

        [Test]
        public async Task CreateDatapoints()
        {
            DataPoint testMqttDataPoint = await UnitOfWork.DataPoints.Find(p => p.Name == "Calcium");
            if (testMqttDataPoint == null)
            {
                List<DataPoint> pts =
                [
                    await CreateFloatMqttDP("WaterQuality", "Calcium", 1),
                    await CreateFloatMqttDP("WaterQuality", "Alkalinity", 1),
                    await CreateFloatMqttDP("WaterQuality", "Magnesium", 1),
                    await CreateFloatMqttDP("WaterQuality", "WaterTemp", 1),
                ];

                await UnitOfWork.DataPoints.CreateAsync(pts);
            }
        }
        private async Task<MQTTDataPoint> CreateFloatMqttDP(string devicename, string name, int offset = 1, string visual = "sun")
        {
            Device dev = await UnitOfWork.Devices.Find(x => x.DeviceName.Equals(devicename));
            return new()
            {
                FK_Device = dev.ID,
                Name = name,
                TopicName = name,
                Offset = offset,
                Icon = visual,
                DataType = DataType.Float
            };
        }
        private async Task<MQTTDataPoint> CreateBooleanMqttDP(string devicename, string name, string visual = "sun")
        {
            Device dev = await UnitOfWork.Devices.Find(x => x.DeviceName.Equals(devicename));
            return new()
            {
                FK_Device = dev.ID,
                Icon = visual,
                Name = name,
                TopicName = name,
                DataType = DataType.Boolean
            };
        }
    }
}
