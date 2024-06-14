using NUnit.Framework;
using ProjectCookie.Services.MQTT;

namespace ProjectCookie.Tests;

public class MqttTest : BaseUnitTest
{
    // TODO private MQTTDataPoint _testDataPoint;
    private MqttDriver _testDriver;
    private float _testSample;

    [Test]
    public async Task SubscribeAndPublishTest()
    {
        await PrepareTestData();
        await Connect();
        await Subscribe();
        await Publish();

        await ValidateAfter(2000);

        await Unsubscribe();
        await Disconnect();
    }

    private async Task PrepareTestData()
    {
        /*
        MQTTDevice d = await UnitOfWork.MQTTDevices.Find(x => x.DeviceName.Contains("WaterQuality"));
        List<MQTTDataPoint> dDPs = await UnitOfWork.MQTTDatapoint.GetForDevice(d.ID);
        Assert.That(dDPs, Has.Count.AtLeast(1));

        _testDataPoint = dDPs[0];
        _testDriver = new(CookieLogger, d, dDPs);
        _testSample = 777;
        */
    }

    private async Task Connect()
    {
        if (_testDriver.MqttClient.IsStarted)
            await _testDriver.Disconnect();

        await _testDriver.Connect();
        Assert.That(_testDriver.MqttClient.IsStarted, Is.True);
    }

    private async Task Subscribe()
    {
        // await _testDriver.Subscribe(_testDataPoint.Name);
        Assert.That(_testDriver.IsSubscribed, Is.True);
    }

    private async Task Publish()
    {
        //TODO await _testDriver.Publish(_testDataPoint.Name, _testSample);
    }

    private async Task ValidateAfter(int delay)
    {
        await Task.Delay(delay);
        /**
                    NumericSample publishedSample =
                                    _testDriver.Measurements[_testDataPoint.Name]
                                        .Select(e => (NumericSample)e)
                                        .First(n => n.Value == 777);

                    Assert.That(publishedSample.Value, Is.EqualTo(777));
                    **/
    }
    
    private async Task Unsubscribe()
    {
        // await _testDriver.Unsubscribe(_testDataPoint.Name);
        Assert.That(_testDriver.IsSubscribed, Is.False);
    }
    
    private async Task Disconnect()
    {
        await _testDriver.Disconnect();
        Assert.That(_testDriver.MqttClient.IsStarted, Is.False);
    }
}