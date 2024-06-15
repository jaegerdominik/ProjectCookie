using ILogger = Serilog.ILogger;

namespace ProjectCookie.Utils.Logging;

public interface ICookieLogger
{
    public ILogger ContextLog<T>(string context) where T : class;
    public ILogger ContextLog<T>() where T : class;

    public Task Init();
}