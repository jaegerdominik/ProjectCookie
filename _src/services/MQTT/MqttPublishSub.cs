using DAL.Entities;
using MQTTnet;
using System.Text.Json;
using ProjectCookie._src.services.MQTT;

namespace Services.Drivers.MQTT
{
    public class MqttPublishSub
    {
        private MQTTDriver _driver;
        public MqttPublishSub(MQTTDriver driver) => _driver = driver;

//TODO
      /**  public async Task PublishNumeric(string topic, NumericSample payload)
        {
            MqttApplicationMessage applicationMessage =
                new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(JsonSerializer.SerializeToUtf8Bytes(payload))
                    .Build();

            await _driver.MqttClient.EnqueueAsync(applicationMessage);
        }**/
    }
}
