using NUnit.Framework;
using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.UnitOfWork;
using ProjectCookie.Utils;
using ProjectCookie.Utils.Logging;

namespace ProjectCookie.Tests;

public class BaseUnitTest
{
    public BaseUnitTest() { }

    protected ISettings Settings;
    protected ISettingsHandler SettingsHandler;
    protected ICookieLogger CookieLogger;
    protected IUnitOfWork UnitOfWork;


    [SetUp]
    public async Task Setup()
    {
        //  var collection = new Mock<IServiceProvider>();

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<ISettings, DataSettings>();
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