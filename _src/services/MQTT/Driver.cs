using DAL.Entities;
using Serilog;
using System.Collections.Concurrent;
using Utilities.Logging;

namespace Services.Drivers
{
    public abstract class Driver
    {

        public ConcurrentDictionary<String, ConcurrentBag<HyperEntity>> Measurements { get; protected set; } = new ConcurrentDictionary<String, ConcurrentBag<HyperEntity>>();
        protected Dictionary<String, DataPoint> DataPoints = new Dictionary<String, DataPoint>();
        protected ILogger log { get; set; }

        public String Name { get; set; }

        public Driver(IAquariumLogger logger, String name)
        {
            this.Name = name;
            log = logger.ContextLog<Driver>(name);
        }


        public Boolean IsConnected { get; protected set; }

        public abstract Task Connect();
        public abstract Task Disconnect();


        public void AddNumericMeasurement(String datapoint, NumericSample measurement)
        {
            if (!Measurements.ContainsKey(datapoint))
            {
                Measurements.TryAdd(datapoint, new ConcurrentBag<HyperEntity>());
            }

            Measurements[datapoint].Add(measurement);
        }

        public void AddBinaryMeasurement(String datapoint, BinarySample measurement)
        {
            if (!Measurements.ContainsKey(datapoint))
            {
                Measurements.TryAdd(datapoint, new ConcurrentBag<HyperEntity>());
            }

            Measurements[datapoint].Add(measurement);
        }


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
            Measurements.Clear();
        }
    }
}
