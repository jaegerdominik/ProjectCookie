using System.Net;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ProjectCookie.Utils.Logging;

public class CookieLogger : ICookieLogger
{
    ISettings Settings = null;
    Boolean IsInitialized = false;
    public CookieLogger(ISettings settings)
    {
        Settings = settings;
    }


    private ILogger _Logger = null;
    public ILogger ContextLog<T>(String context) where T : class
    {
        if (_Logger == null)
        {
            InitLogger();
        }

        ILogger ctx = _Logger.ForContext("Host", Dns.GetHostName()).ForContext("Context", context, destructureObjects: true).ForContext<T>();
        return ctx;
    }


    public ILogger ContextLog<T>() where T : class
    {
        if (_Logger == null)
        {
            InitLogger();
        }

        ILogger ctx = _Logger.ForContext("Host", Dns.GetHostName()).ForContext("Context", "Empty", destructureObjects: true).ForContext<T>();

        return ctx;
    }


    private async Task InitLogger()
    {

        if (!IsInitialized && !String.IsNullOrEmpty(Settings.LoggerSettings))
        {
            Serilog.Debugging.SelfLog.Enable(message =>
            {
                Console.WriteLine(message);
            });

            var configuration = new ConfigurationBuilder().AddJsonStream(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(Settings.LoggerSettings))).Build();
            //TODO Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            _Logger = Log.Logger;
            _Logger.Information("Logger Initialized");
            IsInitialized = true;
        }
    }

    public async Task Init()
    {
        IsInitialized = false;
        await InitLogger();
    }
}