using System.Net;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ProjectCookie.Utils.Logging;

public class CookieLogger : ICookieLogger
{
    private ILogger _logger;
    private bool _isInitialized;

    public CookieLogger() { }


    public ILogger ContextLog<T>(string context) where T : class
    {
        if (_logger == null) InitLogger();

        ILogger ctx = _logger
            .ForContext("Host", Dns.GetHostName())
            .ForContext("Context", context, destructureObjects: true)
            .ForContext<T>();
        
        return ctx;
    }

    public ILogger ContextLog<T>() where T : class
    {
        if (_logger == null) InitLogger();

        ILogger ctx = _logger
            .ForContext("Host", Dns.GetHostName())
            .ForContext("Context", "Empty", destructureObjects: true)
            .ForContext<T>();

        return ctx;
    }


    private async Task InitLogger()
    {
        if (!_isInitialized)
        {
            Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            _logger = Log.Logger;
            _logger.Information("Logger Initialized");
            _isInitialized = true;
        }
    }

    public async Task Init()
    {
        _isInitialized = false;
        await InitLogger();
    }
}