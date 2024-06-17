using System.Text;
using MQTTnet;
using MQTTnet.Client;
using ProjectCookie.DAL.Entities;
using ProjectCookie.Services.BaseInterfaces;
using ProjectCookie.Services.Response;
using ProjectCookie.Utils.Logging;
using Serilog;

namespace ProjectCookie.Services.MQTT;

public class MqttDriver : Driver, IHostedService
{
    private IServiceScopeFactory _scopeFactory;
    
    public IMqttClient MqttClient { get; private set; }
    public MqttClientOptions MqttClientOptions { get; private set; }
    public List<string> TestMessages { get; private set; }

    private MqttConnectSub _connectSub;
    private MqttSubscribeSub _subscribeSub;
    private MqttPublishSub _publishSub;


    public MqttDriver(IServiceScopeFactory scopeFactory, string driverName = "MQTT") : base(driverName)
    {
        _scopeFactory = scopeFactory;
        
        _connectSub = new MqttConnectSub(this);
        _subscribeSub = new MqttSubscribeSub(this);
        _publishSub = new MqttPublishSub(this);
        
        TestMessages = new List<string>();
        
        Random rng = new Random();
        string clientId = $"adswe_client_id_{rng.Next(10000, 100000)}";
        
        MqttClient = new MqttFactory().CreateMqttClient();
        MqttClientOptions = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer(Secret.MqttHost, Secret.MqttPort)
            .WithCredentials(Secret.MqttUsername, Secret.MqttPassword)
            .WithCleanSession()
            .Build();

        MqttClient.ConnectedAsync += async e =>
        {
            Log.Logger.Information("MQTT Client successfully connected.");
            await InitialSubscribe();
        };

        MqttClient.DisconnectedAsync += e =>
        {
            Log.Logger.Information("MQTT Client disconnected.");
            return Task.CompletedTask;
        };

        MqttClient.ApplicationMessageReceivedAsync += e =>
        {
            Log.Logger.Information("MQTT Client received a message.");
            HandleReceivedMessage(e);
            return Task.CompletedTask;
        };
    }


    #region Connect

    public async Task StartAsync(CancellationToken token)
    {
        if (IsConnected) return;
        
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
    
    public async Task HandleReceivedMessage(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        string topic = eventArgs.ApplicationMessage.Topic;
        
        switch (topic)
        {
            case "adswe_mqtt_cookie_test":
                ArraySegment<byte> testData = eventArgs.ApplicationMessage.PayloadSegment;
                string testMessage = Encoding.UTF8.GetString(testData);
                
                TestMessages.Add(testMessage);
                break;
            
            case "adswe_mqtt_cookie_score":
                // Example message: "10|01:20,00|carlos";
                ArraySegment<byte> scoreData = eventArgs.ApplicationMessage.PayloadSegment;
                string scoreMessage = Encoding.UTF8.GetString(scoreData);

                using (IServiceScope scope = _scopeFactory.CreateScope())
                {
                    IGlobalService globalService = scope.ServiceProvider.GetRequiredService<IGlobalService>();

                    string[] fields = scoreMessage.Split('|');
                    int points = int.Parse(fields[0]);
                    string time = fields[1];
                    string username = fields[2];

                    int userId;

                    User? user = await globalService.UserService.GetByName(username);
                    if (user == null)
                    {
                        User newUser = new() { Username = username };
                        ItemResponseModel<User> createdUser = await globalService.UserService.Create(newUser);
                        userId = createdUser.Data.ID;
                    }
                    else
                    {
                        userId = user.ID;
                    }

                    Score mappedScore = new()
                    {
                        Points = points,
                        Timestamp = time,
                        FK_User = userId
                    };
                    
                    await globalService.ScoreService.Create(mappedScore);
                }
                
                break;
            
            case "adswe_mqtt_cookie_user":
                // Example message: "carlos"
                ArraySegment<byte> userData = eventArgs.ApplicationMessage.PayloadSegment;
                string userMessage = Encoding.UTF8.GetString(userData);
                
                User newUser2 = new() { Username = userMessage }; 
                using (IServiceScope scope = _scopeFactory.CreateScope())
                {
                    IGlobalService globalService = scope.ServiceProvider.GetRequiredService<IGlobalService>();
                    ItemResponseModel<User> us = await globalService.UserService.Create(newUser2);
                }
                
                break;
        }
    }
}