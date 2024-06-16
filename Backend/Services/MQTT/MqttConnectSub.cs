using MQTTnet.Client;
using MqttClientDisconnectOptions = MQTTnet.Client.MqttClientDisconnectOptions;

namespace ProjectCookie.Services.MQTT;

public class MqttConnectSub
{
    private MqttDriver _driver;
    public MqttConnectSub(MqttDriver driver) => _driver = driver;

    public async Task StartAsync(CancellationToken token)
    {
        try
        {
            _driver.log.Information($"Connecting...");
            await _driver.MqttClient.ConnectAsync(_driver.MqttClientOptions, token);
        }
        catch (Exception ex)
        {
            _driver.log.Error($"Failed to start MQTT client: {ex.Message}");
        }
    }
    
    public async Task StopAsync(CancellationToken token)
    {
        try
        {
            MqttClientDisconnectOptions disconnectOptions = new MqttClientDisconnectOptions()
            {
                ReasonString = "Normal disconnection",
                Reason = MqttClientDisconnectOptionsReason.NormalDisconnection
            };
            await _driver.MqttClient.DisconnectAsync(disconnectOptions);
        }
        catch (Exception ex)
        {
            _driver.log.Error($"Failed to stop MQTT client: {ex.Message}");
        }
    }
}