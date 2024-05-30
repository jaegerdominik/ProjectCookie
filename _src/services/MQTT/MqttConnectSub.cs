using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace Services.Drivers.MQTT
{
    public class MqttConnectSub
    {
        private MQTTDriver _driver;
        public MqttConnectSub(MQTTDriver driver) => _driver = driver;


        public async Task Connect()
        {
            if (_driver.MqttClient.IsStarted) return;

            MqttClientOptions options =
                new MqttClientOptionsBuilder()
                    .WithClientId("managedClientId")
                    .WithTcpServer(_driver.MqttDevice.Host)
                    .Build();

            ManagedMqttClientOptions mqttClientOptions =
                new ManagedMqttClientOptionsBuilder()
                    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                    .WithClientOptions(options)
                    .Build();

            await _driver.MqttClient.StartAsync(mqttClientOptions);
        }

        public async Task Disconnect()
        {
            if (!_driver.MqttClient.IsStarted) return;

            await _driver.MqttClient.StopAsync();
        }
    }
}
