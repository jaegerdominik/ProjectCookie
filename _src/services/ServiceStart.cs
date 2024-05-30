using System.Collections.Concurrent;
using DAL.Entities;
using DAL.Entities.Devices;
using DAL.UnitOfWork;
using ProjectCookie._src.services.MQTT;
using ProjectCookie._src.utils.Logging;
using Serilog;
using Services.Drivers;
using Utilities.Logging;
using ILogger = Serilog.ILogger;

namespace Services
{
    public class ServiceStart : IServiceStart
    {
        protected ILogger log = null;
        IUnitOfWork UnitOfWork = null;
        Dictionary<String, List<Driver>> Drivers = new Dictionary<String, List<Driver>>();
        System.Timers.Timer timer = null;
        ICookieLogger Logger;
        public ServiceStart(IUnitOfWork unitOfWork, ICookieLogger logger)
        {
            UnitOfWork = unitOfWork;
            this.Logger = logger;
            //log = logger.ContextLog<ServiceStart>();
        }

        public async Task Start()
        {
            timer = new System.Timers.Timer(5000);
            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;
            
            List<MQTTDevice> mqttdevice = await UnitOfWork.MQTTDevices.Get(x => x.Active);

            foreach (MQTTDevice device in mqttdevice)
            {
                List<MQTTDataPoint> dataPoints = await UnitOfWork.MQTTDatapoint.GetForDevice(device.ID);

                MQTTDriver mqtt = new MQTTDriver(Logger, device, dataPoints);

                if (!Drivers.ContainsKey(device.Aquarium))
                {
                    Drivers.Add(device.Aquarium, new List<Driver>());
                    
                    Drivers[device.Aquarium].Add(mqtt);

                    Task.Run(() => mqtt.Connect());
                }
                
            }
        }

        private async void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            await Save();
        }

        public async Task Save()
        {
            /**
            foreach (KeyValuePair<String, List<Driver>> aquarium in Drivers)
            {
                ConcurrentBag<BinarySample> binarySamples = new ConcurrentBag<BinarySample>();
                ConcurrentBag<NumericSample> numericSamples = new ConcurrentBag<NumericSample>();

                foreach (Driver driver in aquarium.Value)
                {
                    foreach (KeyValuePair<String, ConcurrentBag<HyperEntity>> samples in driver.Measurements)
                    {
                        foreach (HyperEntity sample in samples.Value)
                        {
                            if (sample.GetType() == typeof(BinarySample) )
                            {
                                BinarySample binarySample = (BinarySample)sample;
                                await UnitOfWork.Binary.CreateAsync(binarySample);
                            }
                            else
                            {
                                NumericSample numericSample = (NumericSample)sample;
                                await UnitOfWork.Numeric.CreateAsync(numericSample);
                            }
                            
                            
                        }
                    }
                }
              
                
                foreach (Driver dr in aquarium.Value)
                {
                    await dr.Clear();
                }
            }
            
            await UnitOfWork.SaveChangesAsync();
                            **/  
        }



        public async Task Stop()
        {
            //  throw new NotImplementedException();
        }


    }
}
