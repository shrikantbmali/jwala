using System;
using System.Threading.Tasks;
using jwala.philipshuebridge.com.responses.authorization;
using jwala.philipshuebridge.com.responses.resources;
using jwala.philipshuebridge.com.responses.resources.statuses;
using RestSharp;
using Datum = jwala.philipshuebridge.com.responses.resources.statuses.Datum;

namespace jwala.philipshuebridge.com;

public class BridgeCom : Com
{
    private Success _authSuccess;

    public BridgeCom(Bridge bridge, Success authSuccess)
        : base(
            $"https://{bridge.IpAddress}/clip/v2/")
    {
        _authSuccess = authSuccess;
    }

    public async Task<Resource> GetResources()
    {
        var restResponse = await Get(AddKey(new RestRequest("resource/device")));
        return Resource.FromJson(restResponse.Content);
    }

    public async Task<Resource> GetResources(string resource)
    {
        var restResponse = await Get(AddKey(new RestRequest($"resource/{resource}")));
        return Resource.FromJson(restResponse.Content);
    }

    public async Task<Status> GetStatus(string id)
    {
        var restResponse = await Get(AddKey(new RestRequest($"resource/light/{id}")));
        return Status.FromJson(restResponse.Content);
    }

    public async Task<Status> SetStatus(string light, string id, Datum status)
    {
        var restResponse = await Put(
            AddKey(
                new RestRequest($"resource/{light}/{id}")
                    .AddBodyAsJson(status)));

        return Status.FromJson(restResponse.Content);
    }

    private RestRequest AddKey(RestRequest restRequest)
    {
        restRequest.AddHeader("hue-application-key", _authSuccess.Username);
        return restRequest;
    }
}