using DAL.Entities;
using DAL.Entities.Devices;

namespace Tests;

public class CreateDeviceTest : BaseTest

{
    [Test]
        public async Task CreateDevice()
        {
            ModbusDevice device = new ModbusDevice();
            device.Active = true;
            device.Aquarium = "SchiScho2";
            device.DeviceName = "Pump";
            device.DeviceDescription = "Water Pump";
            device.SlaveID = 1;
            device.Host = "127.0.0.1";
            device.Port = 502;


            await UnitOfWork.Devices.CreateAsync(device);
            await UnitOfWork.SaveChangesAsync();
        }



        [Test]
        public async Task CreateDatapoints()
        {
            ModbusDevice device = new ModbusDevice();

            List<DataPoint> pts = new List<DataPoint>
            {
                await CreateFloatModbusDP("Pump", "Pump Current",  0, 1),
                 await CreateFloatModbusDP("Pump", "Pump Voltage",  2, 1),
                 await CreateBooleanModbusDP("Pump", "Pump Status", 1 )
            };


            await UnitOfWork.DataPoints.CreateAsync(pts);
            await UnitOfWork.SaveChangesAsync();


        }


        private async Task<ModbusDataPoint> CreateFloatModbusDP(String devicename, String name, int register, int offset = 1)
        {

            Device dev = await UnitOfWork.Devices.Find(x => x.DeviceName.Equals(devicename));


            ModbusDataPoint pt = new ModbusDataPoint();
            pt.FK_Device = dev.ID;
            pt.Name = name;
            pt.RegisterCount = 2;
            pt.ReadingType = ReadingType.LowToHigh;
            pt.Description = name;
            pt.Register = register;
            pt.RegisterType = RegisterType.HoldingRegister;
            pt.Offset = offset;
            pt.DataType = DataType.Float;
            pt.Icon = "sun";
            return pt;

        }

        private async Task<ModbusDataPoint> CreateBooleanModbusDP(String devicename, String name, int register)
        {
            Device dev = await UnitOfWork.Devices.Find(x => x.DeviceName.Equals(devicename));
            ModbusDataPoint pt = new ModbusDataPoint();
            pt.Name = name;
            pt.Icon = "sun";
            pt.FK_Device = dev.ID;
            pt.Description = name;
            pt.RegisterCount = 1;
            pt.Register = register;
            pt.RegisterType = RegisterType.Coil;
            pt.DataType = DataType.Boolean;
            return pt;

        }

        private async Task<ModbusDataPoint> CreateWriteBooleanModbusDP(String devicename, String name, int register)
        {
            Device dev = await UnitOfWork.Devices.Find(x => x.DeviceName.Equals(devicename));

            ModbusDataPoint pt = new ModbusDataPoint();
            pt.Name = name;
            pt.RegisterCount = 1;
            pt.Register = register;
            pt.RegisterType = RegisterType.WriteSingleCoil;
            pt.DataType = DataType.Boolean;

            return pt;

        }
}