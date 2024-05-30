using NUnit.Framework;
using Tests;
using Utils.Consul;

namespace ProjectCookie._src.tests;

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
            Boolean connected = await cli.Connect();

            Assert.That(connected, Is.True);
        }
    }


    [Test]
    public async Task ReadFromConsul()
    {
        using (ConsulClient cli = new ConsulClient())
        {
            Boolean connected = await cli.Connect();
            Assert.That(connected, Is.True);
            String info = await cli.GetKey("AquariumManagement/Logger");

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