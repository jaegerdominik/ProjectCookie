namespace Services
{
    public interface IServiceStart
    {
        Task Save();
        Task Start();
        Task Stop();
    }
}
