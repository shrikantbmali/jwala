using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using jwala.philipshuebridge.com.responses.authorization;
using jwala.philipshuebridge.com.responses.resources;
using NUnit.Framework;

namespace jwala.philipshuebridge.com.tests;

internal class Bridge_Com_Tests
{
    private string _bridgeLocation;
    private BridgeCom _bridgeCom;

    [SetUp]
    public async Task Setup()
    {
        _bridgeLocation = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "data", "bridges");

        var bridgeAuth = Directory.GetFiles(_bridgeLocation).FirstOrDefault();

        var bridge = (await TestHelper.GetFileText(bridgeAuth)).FromJson<Bridge>();

        var authFileForCurrentBridge = Path.Combine(_bridgeLocation, TestHelper.AuthFolder, bridge.Id);

        if (File.Exists(authFileForCurrentBridge))
        {
            var auth = Auth.FromJson(await TestHelper.GetFileText(authFileForCurrentBridge)).First();

            _bridgeCom = new BridgeCom(bridge, auth.Success);
        }

    }

    [Test]
    public async Task Update_The_IP()
    {
        var bridgeDiscoverer = new BridgeDiscoverer();

        await foreach (var bridge in bridgeDiscoverer.DiscoverBridges())
        {
        }
    }

    [Test]
    public async Task Should_Be_Able_To_Get_device_Resources()
    {
        var restResponse = await _bridgeCom.GetResourceAsync();

        Assert.IsNotNull(restResponse);
        Assert.IsNotNull(restResponse.Data);
        Assert.IsNull(restResponse.Errors);
    }

    [Test]
    public async Task Should_Be_Able_To_Get_Status_Of_All_Sevices()
    {
        var resources = await _bridgeCom.GetResourceAsync();

        var service = resources.Data.SelectMany(
            datum => datum.Services.Where(
                    service => string.Equals(service.Rtype, "light", StringComparison.InvariantCultureIgnoreCase))
                .Select(service => service))
            .FirstOrDefault();

        var status = await _bridgeCom.GetStatus(service.Rid);
    }

    [Test]
    public async Task Should_Be_Able_To_Get_Status_Of_Specified_Service()
    {
        var resources = await _bridgeCom.GetResourceAsync("light");

        var status = await _bridgeCom.GetStatus(resources.Data.First().Id);
    }

    [Test]
    public async Task Should_Be_Able_To_Turn_The_Area_On_Off()
    {
        var resources = await _bridgeCom.GetResourceAsync("light");

        var status = await _bridgeCom.GetStatus(resources.Data.First().Id);

        await _bridgeCom.SetStatus("light", resources.Data.First().Id, new Datum
        {
            On = new On
            {
                OnOn = !status.Data.First().On.OnOn
            }
        });

        await Task.Delay(1000);
    }

    [Test]
    public async Task Should_Be_Able_To_Add_A_Subscription_For_Event_On_A_Resource()
    {
        var resources = await _bridgeCom.GetResourceAsync("light");
        var lightId = resources.Data.First().Id;

        var listener = await _bridgeCom.SubscribeAsync()
            .OnEvent(status =>
            {
                Trace.WriteLine(status.ToJson());
            })
            .StartAsync();

        await Task.Delay(TimeSpan.FromHours(1));

        listener.Stop();
    }
}