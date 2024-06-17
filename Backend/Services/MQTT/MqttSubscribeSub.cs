using MQTTnet.Client;
using Serilog;

namespace ProjectCookie.Services.MQTT;

public class MqttSubscribeSub
{
    private MqttDriver _driver;
    public MqttSubscribeSub(MqttDriver driver) => _driver = driver;


    public void SubscribeToDefaultTopics()
    {
        List<string> topics = ["adswe_mqtt_cookie_message", "adswe_mqtt_cookie_user", "adswe_mqtt_cookie_score"];
        topics.ForEach(SubscribeToTopic);
    }
    
    public async void SubscribeToTopic(string topic)
    {
        MqttClientSubscribeOptions mqttSubscribeOptions = new MqttClientSubscribeOptionsBuilder()
            .WithTopicFilter(f => { f.WithTopic(topic); })
            .Build();
        
        await _driver.MqttClient.SubscribeAsync(mqttSubscribeOptions);
        Log.Logger.Information($"MQTT Client subscribed to topic: {topic}");
    }
    
    public async void UnsubscribeFromTopic(string topic)
    {
        if (!_driver.IsSubscribed) return;
        
        await _driver.MqttClient.UnsubscribeAsync(topic);
        Log.Logger.Information($"MQTT Client unsubscribed from topic: {topic}");
    }
}