using MQTTnet.Client;

namespace ProjectCookie.Services.MQTT;

public class MqttPublishSub
{
    private MqttDriver _driver;
    public MqttPublishSub(MqttDriver driver) => _driver = driver;

    public async Task PublishPayload(string topic, string payload)
    {
        await _driver.MqttClient.PublishStringAsync(topic, payload);
    }
}