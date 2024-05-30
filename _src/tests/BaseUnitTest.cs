using DAL.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ProjectCookie._src.dal.UnitOfWork;
using ProjectCookie._src.tests;
using ProjectCookie._src.utils.Logging;
using Utilities.Logging;
using Utils;

namespace Tests
{
    public class BaseUnitTest
    {
        public BaseUnitTest() { }

        protected ISettings Settings;
        protected ISettingsHandler SettingsHandler;
        protected ICookieLogger CookieLogger;
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
            serviceCollection.AddSingleton<ICookieLogger, CookieLogger>();
            serviceCollection.AddSingleton<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddSingleton<PostgresDbContext>();

            var serviceProvider = serviceCollection.BuildServiceProvider();


            Settings = serviceProvider.GetRequiredService<ISettings>();
            SettingsHandler = serviceProvider.GetRequiredService<ISettingsHandler>();
            await SettingsHandler.Load();
            CookieLogger = serviceProvider.GetRequiredService<ICookieLogger>();
            await CookieLogger.Init();

            UnitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
        }
    }
}