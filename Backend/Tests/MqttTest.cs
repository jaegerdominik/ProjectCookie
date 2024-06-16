using NUnit.Framework;
using ProjectCookie.Services.MQTT;

namespace ProjectCookie.Tests;

[TestFixture, Order(5)]
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

        await Cleanup();
    }

    private async Task PrepareTestData()
    {
        _testDriver = new MqttDriver(CookieLogger);
        _testTopic = "adswe_mqtt_cookie_message";
        _testMessage = "Test MQTT 4 ADSWE";
    }

    private async Task Connect()
    {
        await _testDriver.StartAsync(new CancellationToken());
        Assert.That(_testDriver.MqttClient.IsConnected, Is.True);
    }

    private async Task Subscribe()
    {
        _testDriver.Subscribe(_testTopic);
        Assert.That(_testDriver.IsSubscribed, Is.True);
    }

    private async Task Publish()
    {
        await _testDriver.Publish(_testTopic, _testMessage);
    }

    private async Task ValidateAfter(int delay)
    {
        await Task.Delay(delay);
        
        string publishedMessage = _testDriver.Messages.First(msg => msg == _testMessage);
        Assert.That(publishedMessage, Is.EqualTo(_testMessage));
    }
    
    private async Task Cleanup()
    {
        _testDriver.Unsubscribe(_testTopic);
        await _testDriver.StopAsync(new CancellationToken());
        Assert.That(_testDriver.IsSubscribed, Is.False);
        Assert.That(_testDriver.MqttClient.IsConnected, Is.False);
    }
}