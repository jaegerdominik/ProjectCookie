namespace ProjectCookie.Services.MQTT;

public class MqttConnectSub
{
    private MqttDriver _driver;
    public MqttConnectSub(MqttDriver driver) => _driver = driver;

    public async Task StartAsync()
    {
        try
        {
            await _driver.MqttClient.StartAsync(_driver.MqttClientOptions);
        }
        catch (Exception ex)
        {
            _driver.log.Error($"Failed to start MQTT client: {ex.Message}");
        }
    }
    
    public async Task StopAsync()
    {
        try
        {
            await _driver.MqttClient.StopAsync();
        }
        catch (Exception ex)
        {
            _driver.log.Error($"Failed to stop MQTT client: {ex.Message}");
        }
    }
}