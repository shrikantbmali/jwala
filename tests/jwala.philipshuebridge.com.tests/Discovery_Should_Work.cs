using System.IO;
using System.Threading.Tasks;
using jwala.philipshuebridge.com.responses.authorization;
using NUnit.Framework;

namespace jwala.philipshuebridge.com.tests;

public class Discovery_Should_Work
{
    private string? _bridgeLocation;

    [SetUp]
    public void Setup()
    {
        _bridgeLocation = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "data", "bridges");
    }

    [Test]
    public async Task When_Discovered_Response_Should_Be_Received()
    {
        var discovery = new BridgeDiscoverer();

        await foreach (var bridge in discovery.DiscoverBridges())
        {
            if (!File.Exists(Path.Combine(_bridgeLocation, bridge.Id)))
            {
                var bridgeAuthService = new BridgeAuthService(bridge);

                var auth = await bridgeAuthService.Auth("jwala", "1");

                if (auth.Success != null)
                {
                    await using var fileStream = new StreamWriter(File.Create(Path.Combine(_bridgeLocation, TestHelper.AuthFolder, bridge.Id)));
                    await fileStream.WriteLineAsync(Serialize.ToJson(new[]{auth}));
                }
                else
                {
                    Assert.Fail(auth.Error?.Description ?? "something went wrong");
                }
            }
        }

        Assert.Pass();
    }
}

internal class TestHelper
{
    public const string AuthFolder = "auth";

    public static async Task<string> GetFileText(string? bridgeAuth)
    {
        using var fileReader = new StreamReader(File.Open(bridgeAuth, FileMode.Open));
        string bridgeJson = await fileReader.ReadToEndAsync();
        return bridgeJson;
    }
}