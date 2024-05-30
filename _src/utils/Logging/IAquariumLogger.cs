using Serilog;

namespace Utilities.Logging
{
    public interface IAquariumLogger
    {
        public ILogger ContextLog<T>(String context) where T : class;

        public ILogger ContextLog<T>() where T : class;

        public Task Init();

    }
}
