using System.Linq;
using System.Threading.Tasks;
using jwala.philipshuebridge.com.responses.authorization;
using RestSharp;

namespace jwala.philipshuebridge.com;

public class BridgeAuthService : Com
{
    public BridgeAuthService(Bridge bridge) : base($"https://{bridge.IpAddress}/api")
    {
    }

    public async Task<Auth> Auth(string appName, string instanceName)
    {
        var restResponse = await Post(new RestRequest()
            .AddBodyAsJson(new
            {
                devicetype = $"{appName}#{instanceName}",
                generateclientkey = true
            }));

        return responses.authorization.Auth.FromJson(restResponse.Content).First();
    }
}