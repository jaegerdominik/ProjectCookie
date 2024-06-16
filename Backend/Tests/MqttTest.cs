using System.Text;
using Newtonsoft.Json;
using NUnit.Framework;
using ProjectCookie.Services.MQTT;

namespace ProjectCookie.Tests;

public class MqttTest : BaseUnitTest
{
    private MqttDriver _testDriver;
    private string _testTopic;
    private string _testMessage;

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
        _testDriver = new MqttDriver(CookieLogger);
        _testTopic = "adswe_mqtt_cookie_message";
        _testMessage = "Test MQTT 4 ADSWE";
    }

    private async Task Connect()
    {
        await _testDriver.Connect();
        Assert.That(_testDriver.MqttClient.IsStarted, Is.True);
    }

    private async Task Subscribe()
    {
        _testDriver.Subscribe(_testTopic);
        Assert.That(_testDriver.IsSubscribed, Is.True);
    }

    private async Task Publish()
    {
        byte[] message = Encoding.UTF8.GetBytes(_testMessage);
        await _testDriver.Publish(_testTopic, message);
    }

    private async Task ValidateAfter(int delay)
    {
        await Task.Delay(delay);
        
        string publishedMessage = _testDriver.Messages.First(msg => msg == _testMessage);
        Assert.That(publishedMessage, Is.EqualTo(_testMessage));
    }
    
    private async Task Unsubscribe()
    {
        _testDriver.Unsubscribe(_testTopic);
        Assert.That(_testDriver.IsSubscribed, Is.False);
    }
    
    private async Task Disconnect()
    {
        await _testDriver.Disconnect();
        Assert.That(_testDriver.MqttClient.IsStarted, Is.False);
    }
}