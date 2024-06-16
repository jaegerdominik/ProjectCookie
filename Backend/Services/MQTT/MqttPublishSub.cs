using System.Text;
using System.Text.Json;
using MQTTnet;
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
    
    
    public Task HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        string topic = eventArgs.ApplicationMessage.Topic;
        
        switch (topic)
        {
            case "adswe_mqtt_cookie_message":
                ReadOnlySpan<byte> messageData = eventArgs.ApplicationMessage.PayloadSegment;
                string message = Encoding.UTF8.GetString(messageData);
                
                _driver.Messages.Add(message);
                break;
        }

        return Task.CompletedTask;
    }
}