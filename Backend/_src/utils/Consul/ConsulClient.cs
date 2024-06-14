using Consul;
using System.ComponentModel;
using System.Text;

namespace Utils.Consul
{
    public class ConsulClient : IDisposable
    {

        global::Consul.ConsulClient Client;
        public ConsulClient()
        {

        }

        public async Task<Boolean> Connect()
        {

            String curl = Environment.GetEnvironmentVariable("CONFIG_SERVER");

            if (String.IsNullOrEmpty(curl))
            {
                curl = "http://127.0.0.1:8500";
            }

            Console.WriteLine("Connecting to " + curl);

            try
            {
                ConsulClientConfiguration config = new ConsulClientConfiguration();
                config.Address = new Uri(curl);
                config.WaitTime = TimeSpan.FromSeconds(15); ;

                Client = new global::Consul.ConsulClient(config);
                return true;
            }
            catch (Exception ex)
            {
                WarningException myEx = new WarningException("Consul " + curl + " not reachable");
                Console.WriteLine(myEx);
                Client = null;
                return false;

            }
        }

        public void Dispose()
        {
            if (Client != null)
            {
                Client.Dispose();
            }
        }

        public async Task<String> GetKey(string key)
        {
            if (Client != null)
            {
                var getPair = await Client.KV.Get(key);


                if (getPair?.Response == null)
                {
                    return null;
                }

                String value = Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length);
                return value;
            }

            return null;
        }

        public async Task<Boolean> SetKey(string key, String input)
        {

            if (Client != null)
            {
                KVPair pair = new KVPair(key);
                pair.Key = key;
                byte[] byteArray = Encoding.UTF8.GetBytes(input);
                // byte[] data = Encoding.UTF8.GetBytes(value) ;

                pair.Value = byteArray;

                WriteResult<bool> response = await Client.KV.Put(pair);

                if (response == null || response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    return false;
                }

                return true;

            }

            return false;
        }

    }
}
