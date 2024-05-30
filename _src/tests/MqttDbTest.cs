using DAL.Entities;
using DAL.Entities.Devices;
using NUnit.Framework;
using Tests;

namespace ProjectCookie._src.tests
{
    public class MqttDbTest : BaseUnitTest
    {
        [Test]
        public async Task TestCrud_AllTables()
        {
            MQTTDevice device = GetTestDevice();
            await CreateDevice(device);
            await UpdateDevice(device);

            List<DataPoint> dataPoints = GetTestDataPoints(device);
            await CreateDataPoints(dataPoints);
            
            //NumericSample numericSample = GetNumericTestSample(dataPoints[0]);
            //await CreateNumericSampleEntry(numericSample);

            await DeleteDevice(device);
        }


        #region Device

        private static MQTTDevice GetTestDevice()
            => new()
            {
                Active = true,
                Aquarium = "TestAquarium",
                DeviceName = "TEST",
                DeviceDescription = "Desc",
                Host = "localhost",
                Port = 1883
            };

        private async Task CreateDevice(MQTTDevice device)
        {
            await UnitOfWork.Devices.CreateAsync(device);
            MQTTDevice createdDevice = (MQTTDevice)await UnitOfWork.Devices.Find(d => d.DeviceName == device.DeviceName);
            Assert.That(createdDevice, Is.Not.Null);
        }

        private async Task UpdateDevice(MQTTDevice device)
        {
            device.DeviceName = "Updated Device";
            await UnitOfWork.Devices.Update(device);
            MQTTDevice updatedDevice = (MQTTDevice)await UnitOfWork.Devices.Find(d => d.DeviceName == device.DeviceName);
            Assert.That(updatedDevice, Is.Not.Null);
        }

        private async Task DeleteDevice(MQTTDevice device)
        {
            await UnitOfWork.Devices.Delete(device);
            MQTTDevice deletedDevice = (MQTTDevice)await UnitOfWork.Devices.Find(d => d.DeviceName == device.DeviceName);
            Assert.That(deletedDevice, Is.Null);
        }

        #endregion


        #region DataPoint

        private static List<DataPoint> GetTestDataPoints(MQTTDevice device)
        {
            MQTTDataPoint dp1 = new()
            {
                DataType = DataType.Float,
                Offset = 1,
                Description = "Test1",
                FK_Device = device.ID,
                Name = "dp1"
            };

            MQTTDataPoint dp2 = new()
            {
                DataType = DataType.Boolean,
                Offset = 2,
                Description = "Test2",
                FK_Device = device.ID,
                Name = "dp2"
            };

            return [dp1, dp2];
        }

        private async Task CreateDataPoints(List<DataPoint> dataPoints)
        {
            await UnitOfWork.DataPoints.CreateAsync(dataPoints);
            List<DataPoint> dataPointsInDb = await UnitOfWork.DataPoints.Get(d => d.Description.Contains("Test"));
            Assert.That(dataPointsInDb, Has.Count.EqualTo(2));
        }

        private async Task DeleteFirstDataPointByID(List<DataPoint> dataPoints)
        {
            await UnitOfWork.DataPoints.Delete(dataPoints[0].ID);
            List<DataPoint> dataPointsInDb = await UnitOfWork.DataPoints.Get(d => d.Description.Contains("Test"));
            Assert.That(dataPointsInDb, Has.Count.EqualTo(1));
        }

        private async Task DeleteSecondDataPoint(List<DataPoint> dataPoints)
        {
            await UnitOfWork.DataPoints.Delete(dataPoints[1]);
            List<DataPoint> dataPointsInDb = await UnitOfWork.DataPoints.Get(d => d.Description.Contains("Test"));
            Assert.That(dataPointsInDb, Is.Empty);
        }

        #endregion

/**
        #region Numeric

        private static NumericSample GetNumericTestSample(DataPoint dataPoint)
            => new()
            {
                Status = 1,
                Time = DateTimeOffset.UtcNow,
                FK_Datapoint = dataPoint.ID,
                Value = (float)new Random().NextDouble()
            };

        private async Task CreateNumericSampleEntry(NumericSample numericSample)
        {
            await UnitOfWork.Numeric.CreateAsync(numericSample);
            NumericSample lastValue = await UnitOfWork.Numeric.GetLastValue(numericSample.FK_Datapoint);
            Assert.That(lastValue.Value, Is.EqualTo(numericSample.Value));
        }

        #endregion


        #region Binary

        private static List<BinarySample> GetBinarySamples(DataPoint dataPoint)
        {
            BinarySample smp1 = new()
            {
                Status = 1,
                Time = DateTimeOffset.UtcNow,
                FK_Datapoint = dataPoint.ID,
                Value = true
            };

            BinarySample smp2 = new()
            {
                Status = 2,
                Time = DateTimeOffset.UtcNow,
                FK_Datapoint = dataPoint.ID,
                Value = false
            };

            return [smp1, smp2];
        }

        private async Task CreateBinarySampleEntries(List<BinarySample> binarySamples)
        {
            await UnitOfWork.Binary.CreateAsync(binarySamples);
            List<BinarySample> binarySamplesInDb = await UnitOfWork.Binary.GetValues(binarySamples[0].FK_Datapoint, DateTimeOffset.MinValue, DateTimeOffset.MaxValue);
            Assert.That(binarySamplesInDb, Has.Count.EqualTo(2));
        }

        #endregion
            **/
    }
}
