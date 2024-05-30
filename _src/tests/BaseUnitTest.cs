using DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Utilities.Logging;
using Utils;

namespace Tests
{
    public class BaseUnitTest
    {
        public BaseUnitTest() { }

        protected ISettings Settings;
        protected ISettingsHandler SettingsHandler;
        protected IAquariumLogger AquariumLogger;
        protected IUnitOfWork UnitOfWork;
        // protected IAuthentication Authentication;
        //protected IPasswordHasher PasswordHasher;


        [SetUp]
        public async Task Setup()
        {
            //  var collection = new Mock<IServiceProvider>();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ISettings, DataSettings>();
            serviceCollection.AddSingleton<ISettingsHandler, TestSettingsHandler>();
            serviceCollection.AddSingleton<IAquariumLogger, AquariumLogger>();
            serviceCollection.AddSingleton<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddSingleton<TimeScaleContext>();

            var serviceProvider = serviceCollection.BuildServiceProvider();


            Settings = serviceProvider.GetRequiredService<ISettings>();
            SettingsHandler = serviceProvider.GetRequiredService<ISettingsHandler>();
            await SettingsHandler.Load();
            AquariumLogger = serviceProvider.GetRequiredService<IAquariumLogger>();
            await AquariumLogger.Init();

            UnitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }
    }
}