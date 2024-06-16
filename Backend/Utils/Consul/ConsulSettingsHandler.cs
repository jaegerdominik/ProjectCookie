using System.ComponentModel;
using Newtonsoft.Json;
using ProjectCookie.Utils.SettingItems;

namespace ProjectCookie.Utils.Consul;

public class ConsulSettingsHandler : ISettingsHandler
{
    ISettings Settings { get; set; }
    public ConsulSettingsHandler(ISettings settings)
    {
        Settings = settings;
    }
    public async Task Load()
    {
        using (ConsulClient cli = new ConsulClient())
        {
            bool conn = await cli.Connect();

            if (conn)
            {
                try
                {
                    string logger = await cli.GetKey("CookieData/Logger");
                    string db = await cli.GetKey("CookieData/Database");
                    TimeScaleDBSettings dbSettings = JsonConvert.DeserializeObject<TimeScaleDBSettings>(db);
                    
                    Settings.LoggerSettings = logger;
                    Settings.TimeScaleDBSettings = dbSettings;
                }
                catch (Exception ex)
                {
                    WarningException myEx = new WarningException("Error during reading configuration", ex);
                    Console.WriteLine(myEx);
                }
            }
        }
    }
    
    /* Consul Key/Value Pairs */
    /*
    |** CookieData/Database:
{
"DatabaseName": "johann",
"Port": "5433",
"Password": "pass",
"Username": "admin",
"Server": "host.docker.internal"
}
    */
    /*
    |** CookieData/Logger:
{
  "Host": {
    "Port": "80",
    "Protocol": "http",
    "SettingsFile": "SimulationSettings/settings.json"
  },
  "Serilog": {
    "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File", "Serilog.Enrichers.Environment" ],
    "MinimumLevel": {
      "Default": "Verbose",
      "Override": {
        "Microsoft": "Debug",
        "System": "Debug"
      }
    },
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/log.log",
          "rollingInterval": "Day"
        }
      }
    ],
    "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ]
  }
}
    */
}
