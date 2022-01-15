using System.Linq;
using System.Threading.Tasks;
using RestSharp;

namespace jwala.philipshuebridge.com;

public abstract class Com
{
    private readonly RestClient _restClient;

    protected Com(string baseUri) : this(baseUri, null)
    {
    }

    protected Com(string baseUri, params (string key, string value)[] headers)
    {
        _restClient = new RestClient(new RestClientOptions(baseUri)
        {
            RemoteCertificateValidationCallback = (_, _, _, _) => true,
        });

        _restClient.AddDefaultHeader("Content-Type", "application/json");
            
        if (headers?.Any() ?? false)
        {
            foreach (var header in headers)
            {
                _restClient.AddDefaultHeader(header.key, header.value);
            }
        }
    }

    protected async Task<RestResponse> Post(RestRequest restRequest)
    {
        return await _restClient.PostAsync(restRequest);
    }

    protected async Task<RestResponse> Get(RestRequest restRequest)
    {
        return await _restClient.GetAsync(restRequest);
    }
    
    protected async Task<RestResponse> Put(RestRequest restRequest)
    {
        return await _restClient.PutAsync(restRequest);
    }
}