using MQTTnet;
using MQTTnet.Client;

namespace ProjectCookie.Services.MQTT;

public class MqttSubscribeSub
{
    private MqttDriver _driver;
    public MqttSubscribeSub(MqttDriver driver) => _driver = driver;


    public async Task SubscribeTopic(string topic)
    {
        await _driver.MqttClient.SubscribeAsync([
            new MqttTopicFilterBuilder()
                .WithTopic(topic)
                .Build()
        ]);

        _driver.MqttClient.ApplicationMessageReceivedAsync += HandleReceivedMessage;
    }

    public async Task UnsubscribeTopic(string topic)
    {
        await _driver.MqttClient.UnsubscribeAsync([topic]);

        _driver.MqttClient.ApplicationMessageReceivedAsync -= HandleReceivedMessage;
    }


    private Task HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        //TODO
        /** string topic = eventArgs.ApplicationMessage.Topic;
         DataPoint dataPoint = _driver.MqttDataPoints.First(dp => dp.TopicName == topic);

         ReadOnlySpan<byte> messageData = eventArgs.ApplicationMessage.PayloadSegment;
         NumericSample sample = JsonSerializer.Deserialize<NumericSample>(messageData);

         _driver.AddNumericMeasurement(dataPoint.Name, sample);
         **/

        return Task.CompletedTask;
    }
}