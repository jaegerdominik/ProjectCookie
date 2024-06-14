using System.Text.Json;
using MQTTnet;

namespace ProjectCookie.Services.MQTT;

public class MqttPublishSub
{
    private MqttDriver _driver;
    public MqttPublishSub(MqttDriver driver) => _driver = driver;

    public async Task PublishPayload(string topic, byte[] payload)
    {
        MqttApplicationMessage applicationMessage =
            new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(JsonSerializer.SerializeToUtf8Bytes(payload))
                .Build();

        await _driver.MqttClient.EnqueueAsync(applicationMessage);
    }
}