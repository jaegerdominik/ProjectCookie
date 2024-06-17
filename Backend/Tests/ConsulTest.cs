using NUnit.Framework;
using ProjectCookie.Utils.Consul;

namespace ProjectCookie.Tests;

[TestFixture, Order(2)]
public class ConsulTest : BaseUnitTest
{
    [Test]
    public void ConsulReadTest()
    {
        Assert.That(Settings.LoggerSettings, Is.Not.Null);
    }
    
    [Test]
    public async Task ConnectToConsulTest()
    {
        using ConsulClient cli = new();
        
        bool connected = await cli.Connect();
        Assert.That(connected, Is.True);
    }

    [Test]
    public async Task ReadFromConsulTest()
    {
        using ConsulClient cli = new();
        
        bool connected = await cli.Connect();
        Assert.That(connected, Is.True);
            
        string? info = await cli.GetKey("CookieData/Logger");
        Assert.That(info, Is.Not.Null);
    }

    [Test]
    public async Task ReadFromConsulWithHandlerTest()
    {
        await SettingsHandler.Load();
        Assert.That(Settings.LoggerSettings, Is.Not.Null);
    }
}