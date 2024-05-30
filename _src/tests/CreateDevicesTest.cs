using DAL.Entities;
using DAL.Entities.Devices;

namespace Tests
{
    public class CreateDevicesTest : BaseUnitTest
    {
        private static ModbusDevice TestModbusDevice
            => new()
            {
                Active = true,
                Aquarium = "SchiScho2",
                DeviceName = "Pump",
                DeviceDescription = "Water Pump",
                SlaveID = 1,
                Host = "127.0.0.1",
                Port = 502
            };
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
            Device testModbusDevice = await UnitOfWork.Devices.Find(d => d.DeviceName == "Pump");
            if (testModbusDevice == null) await UnitOfWork.Devices.CreateAsync(TestModbusDevice);

            Device testMqttDevice = await UnitOfWork.Devices.Find(d => d.DeviceName == "WaterQuality");
            if (testMqttDevice == null) await UnitOfWork.Devices.CreateAsync(TestMqttDevice);
        }

        [Test]
        public async Task CreateDatapoints()
        {
            DataPoint testModbusDataPoint = await UnitOfWork.DataPoints.Find(p => p.Name == "Pump Current");
            if (testModbusDataPoint == null)
            {
                List<DataPoint> pts =
                [
                    await CreateFloatModbusDP("Pump", "Pump Current",  0, 1),
                    await CreateFloatModbusDP("Pump", "Pump Voltage",  2, 1),
                    await CreateBooleanModbusDP("Pump", "Pump Status", 1 )
                ];

                await UnitOfWork.DataPoints.CreateAsync(pts);
            }

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


        private async Task<ModbusDataPoint> CreateFloatModbusDP(string devicename, string name, int register, int offset = 1)
        {

            Device dev = await UnitOfWork.Devices.Find(x => x.DeviceName.Equals(devicename));
            return new()
            {
                FK_Device = dev.ID,
                Name = name,
                RegisterCount = 2,
                ReadingType = ReadingType.LowToHigh,
                Description = name,
                Register = register,
                RegisterType = RegisterType.HoldingRegister,
                Offset = offset,
                DataType = DataType.Float,
                Icon = "sun"
            };

        }
        private async Task<ModbusDataPoint> CreateBooleanModbusDP(string devicename, string name, int register)
        {
            Device dev = await UnitOfWork.Devices.Find(x => x.DeviceName.Equals(devicename));
            return new()
            {
                Name = name,
                Icon = "sun",
                FK_Device = dev.ID,
                Description = name,
                RegisterCount = 1,
                Register = register,
                RegisterType = RegisterType.Coil,
                DataType = DataType.Boolean
            };
        }
        private async Task<ModbusDataPoint> CreateWriteBooleanModbusDP(string devicename, string name, int register)
        {
            Device dev = await UnitOfWork.Devices.Find(x => x.DeviceName.Equals(devicename));
            return new()
            {
                Name = name,
                RegisterCount = 1,
                Register = register,
                RegisterType = RegisterType.WriteSingleCoil,
                DataType = DataType.Boolean
            };
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
