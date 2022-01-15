using System.Collections.Generic;
using System.Threading.Tasks;
using jwala.philipshuebridge.com.responses.configurations;
using jwala.philipshuebridge.com.responses.discovery;
using RestSharp;

namespace jwala.philipshuebridge.com;

public sealed class BridgeDiscoverer
{
    public async IAsyncEnumerable<Bridge> DiscoverBridges()
    {
        var discoveryResponses = await Discover();

        foreach (var discoveryResponse in DiscoveryResponse.FromJson(discoveryResponses.Content))
        {
            var config = await Config(discoveryResponse.Internalipaddress);

            yield return CreateBridge(discoveryResponse, config);
        }
    }

    private static Bridge CreateBridge(DiscoveryResponse discoveryResponse, ConfigResponse config)
    {
        return new Bridge(discoveryResponse.Id, discoveryResponse.Internalipaddress, config.Name);
    }

    private static async Task<RestResponse> Discover()
    {
        var restClient = new RestClient("https://discovery.meethue.com");

        var discoveryResponses = await restClient.GetAsync(new RestRequest());
        return discoveryResponses;
    }

    public async Task<IEnumerable<Bridge>> StartDiscoveryAsync()
    {
        var bridges = new List<Bridge>();

        var discoveryResponses = await Discover();

        foreach (var discoveryResponse in DiscoveryResponse.FromJson(discoveryResponses.Content))
        {
            bridges.Add(CreateBridge(discoveryResponse, await Config(discoveryResponse.Internalipaddress)));
        }

        return bridges;
    }

    private static async Task<ConfigResponse> Config(string discoveryResponseId)
    {
        var restClient = new RestClient($"http://{discoveryResponseId}/api/0/config");

        var restResponse = await restClient.GetAsync(new RestRequest());

        return ConfigResponse.FromJson(restResponse.Content);
    }
}