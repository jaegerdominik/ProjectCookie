using MQTTnet.Client;

namespace ProjectCookie.Services.MQTT;

public class MqttSubscribeSub
{
    private MqttDriver _driver;
    public MqttSubscribeSub(MqttDriver driver) => _driver = driver;


    public void SubscribeToDefaultTopics()
    {
        List<string> topics = ["adswe_mqtt_cookie_message"];
        topics.ForEach(SubscribeToTopic);
    }
    
    public async void SubscribeToTopic(string topic)
    {
        MqttClientSubscribeOptions mqttSubscribeOptions = new MqttClientSubscribeOptionsBuilder()
            .WithTopicFilter(f => { f.WithTopic(topic); })
            .Build();
        
        await _driver.MqttClient.SubscribeAsync(mqttSubscribeOptions);
    }
    
    public async void UnsubscribeFromTopic(string topic)
    {
        if (!_driver.IsSubscribed) return;
        
        await _driver.MqttClient.UnsubscribeAsync(topic);
    }
}