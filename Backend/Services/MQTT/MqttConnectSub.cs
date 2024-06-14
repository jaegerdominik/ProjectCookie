using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;

namespace ProjectCookie.Services.MQTT;

public class MqttConnectSub
{
    private MqttDriver _driver;
    public MqttConnectSub(MqttDriver driver) => _driver = driver;


    public async Task Connect()
    {
        if (_driver.MqttClient.IsStarted) return;

        MqttClientOptions options =
            new MqttClientOptionsBuilder()
                .WithClientId("managedClientId")
                // TODO .WithTcpServer(_driver.hostname)
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