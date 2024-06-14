namespace ProjectCookie.Services.BaseInterfaces;

public interface IServiceStart
{
    Task Save();
    Task Start();
    Task Stop();
}