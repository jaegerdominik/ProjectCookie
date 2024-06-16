using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using ProjectCookie.Utils.Logging;

namespace ProjectCookie.Services.MQTT;

public class MqttDriver : Driver
{
    public IManagedMqttClient MqttClient { get; private set; }
    public ManagedMqttClientOptions MqttClientOptions { get; private set; }
    public List<string> Messages { get; private set; }
    public bool IsSubscribed { get; private set; }

    private readonly string _host = "dmt.fh-joanneum.at";
    private readonly int _port = 1883;

    private MqttConnectSub _connectSub;
    private MqttSubscribeSub _subscribeSub;
    private MqttPublishSub _publishSub;


    public MqttDriver(ICookieLogger logger, string driverName = "MQTT") : base(logger, driverName)
    {
        _connectSub = new MqttConnectSub(this);
        _subscribeSub = new MqttSubscribeSub(this);
        _publishSub = new MqttPublishSub(this);
        
        Messages = new List<string>();
        
        MqttClient = new MqttFactory().CreateManagedMqttClient();
        Random rng = new Random();
        MqttClientOptions = new ManagedMqttClientOptionsBuilder()
            .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
            .WithClientOptions(new MqttClientOptionsBuilder()
                .WithClientId($"adswe_managedClientId_{rng.Next(10000, 100000)}")
                .WithTcpServer(_host, _port)
                .WithCredentials(Secret.MqttUsername, Secret.MqttUsername)
                .WithCleanSession()
                .Build())
            .Build();

        MqttClient.ConnectedAsync += async e =>
        {
            log.Information("Connected to MQTT server. Subscribing...");
            await InitialSubscribe();
        };

        MqttClient.DisconnectedAsync += e =>
        {
            log.Warning("Disconnected from MQTT server.");
            return Task.CompletedTask;
        };

        MqttClient.ApplicationMessageReceivedAsync += e =>
        {
            _publishSub.HandleReceivedMessage(e);
            return Task.CompletedTask;
        };
    }


    #region Connect

    public async Task Connect()
    {
        if (IsConnected) return;
        
        await _connectSub.StartAsync();
        IsConnected = MqttClient.IsConnected;
        log.Information($"The MQTT client is connected: {IsConnected}");
    }

    public async Task Disconnect()
    {
        if (!IsConnected) return;

        await _connectSub.StopAsync();
        IsConnected = MqttClient.IsConnected;
        IsSubscribed = false;
        log.Information($"The MQTT client is disconnected: {!IsConnected}");
    }

    #endregion
    
    #region Subscribe

    private Task InitialSubscribe()
    {
        _subscribeSub.SubscribeToDefaultTopics();
        IsSubscribed = true;
        return Task.CompletedTask;
    }

    public void Subscribe(string topic)
    {
        _subscribeSub.SubscribeToTopic(topic);
        IsSubscribed = true;
        log.Information($"Subscribed to topic {topic}");
    }

    public Task Unsubscribe(string topic)
    {
        _subscribeSub.UnsubscribeFromTopic(topic);
        log.Information($"Unsubscribed from topic {topic}");
        return Task.CompletedTask;
    }
    
    #endregion

    #region Publish

    public async Task Publish(string topic, byte[] payload)
    {
        await _publishSub.PublishPayload(topic, payload);
        log.Information($"MQTT client published payload: {payload} to topic: {topic}");
    }

    #endregion
}