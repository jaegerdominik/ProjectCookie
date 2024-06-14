using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using ProjectCookie.Utils.Logging;

namespace ProjectCookie.Services.MQTT;

public class MqttDriver : Driver
{
    public IManagedMqttClient MqttClient { get; private set; }
    public bool IsSubscribed { get; private set; }

    private MqttConnectSub _connectSub;
    private MqttSubscribeSub _subscribeSub;
    private MqttPublishSub _publishSub;


    public MqttDriver(ICookieLogger logger, string driverName = "MQTT") : base(logger, driverName)
    {
        MqttClientOptions options =
            new MqttClientOptionsBuilder()
                .WithClientId("mqttManagedClient")
                // TODO .WithTcpServer(MqttDevice.Host)
                .Build();

        MqttClient = new MqttFactory().CreateManagedMqttClient();
        MqttClient.StartAsync(new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(options)
            .Build());
        
        _connectSub = new MqttConnectSub(this);
        _subscribeSub = new MqttSubscribeSub(this);
        _publishSub = new MqttPublishSub(this);
    }


    public async override Task Connect()
    {
        await _connectSub.Connect();
        IsConnected = MqttClient.IsConnected;
        log.Information("The MQTT client is connected.");
    }
    public async override Task Disconnect()
    {
        await _connectSub.Disconnect();
        IsConnected = MqttClient.IsConnected;
        log.Information("The MQTT client is disconnected.");
    }

    public async Task Subscribe(string topic)
    {
        await _subscribeSub.SubscribeTopic(topic);
        IsSubscribed = true;
        log.Information($"MQTT client subscribed to topic: {topic}");
    }

    public async Task Unsubscribe(string topic)
    {
        await _subscribeSub.UnsubscribeTopic(topic);
        IsSubscribed = false;
        log.Information($"MQTT client unsubscribed to topic: {topic}");
    }

    public async Task Publish(string topic, byte[] payload)
    {
        await _publishSub.PublishPayload(topic, payload);
        log.Information($"MQTT client published payload: {payload} to topic: {topic}");
    }
}