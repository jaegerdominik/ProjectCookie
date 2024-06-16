using MQTTnet.Extensions.ManagedClient;

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
        await _driver.MqttClient.SubscribeAsync(topic);
    }
    
    public async void UnsubscribeFromTopic(string topic)
    {
        await _driver.MqttClient.UnsubscribeAsync(topic);
    }
}