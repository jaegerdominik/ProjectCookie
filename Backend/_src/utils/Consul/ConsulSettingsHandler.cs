using DAL.UnitOfWork;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Utils.Consul
{
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
                        String logger = await cli.GetKey("AquariumData/Logger");
                        Settings.LoggerSettings = logger;
                        String mongodb = await cli.GetKey("AquariumData/Database");

                        TimeScaleDBSettings mongoDBSettings = JsonConvert.DeserializeObject<TimeScaleDBSettings>(mongodb);
                        Settings.TimeScaleDBSettings = mongoDBSettings;
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
}
