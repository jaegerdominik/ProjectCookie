using System.Collections.Concurrent;
using DAL.Entities;
using ProjectCookie._src.utils.Logging;
using Utilities.Logging;
using ILogger = Serilog.ILogger;

namespace ProjectCookie._src.services.MQTT
{
    public abstract class Driver
    {
        protected Dictionary<String, DataPoint> DataPoints = new Dictionary<String, DataPoint>();
        protected ILogger log { get; set; }

        public String Name { get; set; }

        public Driver(ICookieLogger logger, String name)
        {
            this.Name = name;
           //TODO log = logger.ContextLog<Driver>(name);
        }


        public Boolean IsConnected { get; protected set; }

        public abstract Task Connect();
        public abstract Task Disconnect();
        
        public void AddDataPoint(String name, DataPoint pt)
        {
            if (!DataPoints.ContainsKey(name))
            {
                DataPoints.Add(name, pt);
            }
        }

        public DataPoint GetDataPoint(String name)
        {
            if (!DataPoints.ContainsKey(name))
            {
                return null;
            }
            else
            {
                return DataPoints[name];
            }
        }

        public async Task Clear()
        {
        }
    }
}
