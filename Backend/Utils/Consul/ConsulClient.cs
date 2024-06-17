using System.ComponentModel;
using System.Text;
using Consul;

namespace ProjectCookie.Utils.Consul;

public class ConsulClient : IDisposable
{
    global::Consul.ConsulClient Client;
    public ConsulClient() { }

    public async Task<Boolean> Connect()
    {
        string curl = Environment.GetEnvironmentVariable("CONFIG_SERVER");
        if (string.IsNullOrEmpty(curl)) curl = "http://127.0.0.1:8500";

        Console.WriteLine("Connecting to " + curl);

        try
        {
            ConsulClientConfiguration config = new();
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

    public async Task<string?> GetKey(string key)
    {
        if (Client == null) return null;
        
        QueryResult<KVPair>? getPair = await Client.KV.Get(key);
        if (getPair?.Response == null) return null;

        string value = Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length);
        return value;
    }

    public async Task<bool> SetKey(string key, string input)
    {
        if (Client == null) return false;
        
        byte[] byteArray = Encoding.UTF8.GetBytes(input);
        KVPair pair = new(key);
        pair.Key = key;
        pair.Value = byteArray;

        WriteResult<bool> response = await Client.KV.Put(pair);
        return response is { StatusCode: System.Net.HttpStatusCode.OK };
    }
}