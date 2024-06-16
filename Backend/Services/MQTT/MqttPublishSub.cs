using System.Text;
using System.Text.Json;
using MQTTnet;
using MQTTnet.Client;

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
    
    
    public Task HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        string topic = eventArgs.ApplicationMessage.Topic;
        _driver.log.Information($"Received message on topic: {topic}");
        
        switch (topic)
        {
            case "adswe_mqtt_cookie_message":
                ReadOnlySpan<byte> messageData = eventArgs.ApplicationMessage.PayloadSegment;
                string message = Encoding.UTF8.GetString(messageData);
                
                _driver.log.Information($"Received message: {message}");
                _driver.Messages.Add(message);
                break;
        }

        return Task.CompletedTask;
    }
}