using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.UnitOfWork;
using ProjectCookie.Utils;
using ProjectCookie.Utils.Consul;
using ProjectCookie.Utils.Logging;

namespace ProjectCookie.Tests;

[TestFixture]
public class BaseUnitTest
{
    public BaseUnitTest() { }

    protected ISettings Settings;
    protected ISettingsHandler SettingsHandler;
    protected ICookieLogger CookieLogger;
    protected IUnitOfWork UnitOfWork;

    protected ServiceCollection _services;
    protected ServiceProvider _serviceProvider;

    
    [SetUp]
    public virtual async Task Setup()
    {
        _services = new ServiceCollection();
        _CollectServices();
        _serviceProvider = _services.BuildServiceProvider();

        Settings = _serviceProvider.GetRequiredService<ISettings>();
        SettingsHandler = _serviceProvider.GetRequiredService<ISettingsHandler>();
        await SettingsHandler.Load();
        CookieLogger = _serviceProvider.GetRequiredService<ICookieLogger>();
        await CookieLogger.Init();
        
        UnitOfWork = _serviceProvider.GetRequiredService<IUnitOfWork>();
    }

    [TearDown]
    public virtual async Task TearDown()
    {
        _serviceProvider.Dispose();
    }


    protected virtual void _CollectServices()
    {
        _services.AddSingleton<ISettings, DataSettings>();
        _services.AddSingleton<ISettingsHandler, ConsulSettingsHandler>();
        _services.AddSingleton<ICookieLogger, CookieLogger>();
        _services.AddSingleton<PostgresDbContext>();
        
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        _services.AddDbContext<PostgresDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
        _services.AddSingleton<IUnitOfWork, UnitOfWork>();
    }

        
    [Test]
    public async Task TestSetup()   
    {
        Assert.That(_services.Count, Is.GreaterThan(0));
    }
}