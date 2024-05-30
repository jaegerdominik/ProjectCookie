namespace ProjectCookie._src.utils.Logging;

public interface ICookieLogger
{
    public Serilog.ILogger ContextLog<T>(String context) where T : class;

    public Serilog.ILogger ContextLog<T>() where T : class;

    public Task Init();

}