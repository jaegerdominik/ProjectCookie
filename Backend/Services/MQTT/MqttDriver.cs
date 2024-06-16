using MQTTnet;
using MQTTnet.Client;
using ProjectCookie.Utils.Logging;
using Serilog;

namespace ProjectCookie.Services.MQTT;

public class MqttDriver : Driver, IHostedService
{
    private IServiceScopeFactory _scopeFactory;
    public IMqttClient MqttClient { get; private set; }
    public MqttClientOptions MqttClientOptions { get; private set; }
    public List<string> Messages { get; private set; }

    private readonly string _host = "dmt.fh-joanneum.at";
    private readonly int _port = 1883;

    private MqttConnectSub _connectSub;
    private MqttSubscribeSub _subscribeSub;
    private MqttPublishSub _publishSub;


    public MqttDriver(IServiceScopeFactory scopeFactory, string driverName = "MQTT") : base(driverName)
    {
        _scopeFactory = scopeFactory;
        
        _connectSub = new MqttConnectSub(this);
        _subscribeSub = new MqttSubscribeSub(this);
        _publishSub = new MqttPublishSub(this);
        
        Messages = new List<string>();
        
        Random rng = new Random();
        string clientId = $"adswe_client_id_{rng.Next(10000, 100000)}";
        
        MqttClient = new MqttFactory().CreateMqttClient();
        MqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer(_host, _port)
            .WithCredentials(Secret.MqttUsername, Secret.MqttPassword)
            .WithCleanSession()
            .Build();

        MqttClient.ConnectedAsync += async e =>
        {
            await InitialSubscribe();
        };

        MqttClient.DisconnectedAsync += e =>
        {
            return Task.CompletedTask;
        };

        MqttClient.ApplicationMessageReceivedAsync += e =>
        {
            _publishSub.HandleReceivedMessage(e);
            return Task.CompletedTask;
        };
    }


    #region Connect

    public async Task StartAsync(CancellationToken token)
    {
        using (IServiceScope scope = _scopeFactory.CreateScope())
        {
            ICookieLogger logger = scope.ServiceProvider.GetRequiredService<ICookieLogger>();
            logger.ContextLog<MqttDriver>();
            Log.Logger.Information("MqttDriver started");
            
            if (IsConnected) return;
            
            await _connectSub.StartAsync(token);
            IsConnected = MqttClient.IsConnected;
            logger.ContextLog<MqttDriver>($"The MQTT client is connected: {IsConnected}");
            Log.Logger.Information($"The MQTT client is connected: {IsConnected}");
        }
    }

    public async Task StopAsync(CancellationToken token)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ICookieLogger>();
            logger.ContextLog<MqttDriver>("MqttDriver stopped");

            if (!IsConnected) return;

            await _connectSub.StopAsync(token);
            IsConnected = MqttClient.IsConnected;
            IsSubscribed = false;
            logger.ContextLog<MqttDriver>($"The MQTT client is disconnected: {!IsConnected}");
        }
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
    }

    public Task Unsubscribe(string topic)
    {
        _subscribeSub.UnsubscribeFromTopic(topic);
        return Task.CompletedTask;
    }
    
    #endregion

    #region Publish

    public async Task Publish(string topic, string payload)
    {
        await _publishSub.PublishPayload(topic, payload);
    }

    #endregion
}