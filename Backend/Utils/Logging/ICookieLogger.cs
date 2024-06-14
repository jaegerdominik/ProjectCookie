namespace ProjectCookie.Utils.Logging;

public interface ICookieLogger
{
    public Serilog.ILogger ContextLog<T>(string context) where T : class;

    public Serilog.ILogger ContextLog<T>() where T : class;

    public Task Init();
}