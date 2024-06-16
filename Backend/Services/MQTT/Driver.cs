namespace ProjectCookie.Services.MQTT;

public abstract class Driver
{
    public string Name { get; set; }
    public bool IsConnected { get; protected set; }
    public bool IsSubscribed { get; protected set; }

    public Driver(string name)
    {
        Name = name;
    }
}