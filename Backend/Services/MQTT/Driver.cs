﻿using ProjectCookie.Utils.Logging;
using ILogger = Serilog.ILogger;

namespace ProjectCookie.Services.MQTT;

public abstract class Driver
{
    public string Name { get; set; }
    public bool IsConnected { get; protected set; }
    protected ILogger log { get; set; }

    public Driver(ICookieLogger logger, string name)
    {
        Name = name;
        log = logger.ContextLog<Driver>(name);
    }


    public abstract Task Connect();
    public abstract Task Disconnect();
}