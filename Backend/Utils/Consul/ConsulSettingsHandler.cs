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
            Boolean conn = await cli.Connect();

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
}