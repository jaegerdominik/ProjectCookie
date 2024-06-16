using NUnit.Framework;
using ProjectCookie.Utils.Consul;

namespace ProjectCookie.Tests;

public class ConsulTest : BaseUnitTest
{
    [Test]
    public async Task ConsulReadTest()
    {
        Assert.That(Settings.LoggerSettings, Is.Not.Null);
    }
    
    [Test]
    public async Task ConnectToConsul()
    {
        using (ConsulClient cli = new ConsulClient())
        {
            bool connected = await cli.Connect();
            Assert.That(connected, Is.True);
        }
    }

    [Test]
    public async Task ReadFromConsul()
    {
        using (ConsulClient cli = new ConsulClient())
        {
            bool connected = await cli.Connect();
            Assert.That(connected, Is.True);
            
            string info = await cli.GetKey("CookieData/Logger");
            Assert.That(info, Is.Not.Null);
        }
    }

    [Test]
    public async Task ReadFromConsulWithHandler()
    {
        await SettingsHandler.Load();
        Assert.That(Settings.LoggerSettings, Is.Not.Null);
    }
}