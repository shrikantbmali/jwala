using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using jwala.philipshuebridge.com.responses.resources.messages;

namespace jwala.philipshuebridge.com;

public class StreamEventListener
{
    private Action<Message> _action;
    private readonly string _uri;
    private readonly HttpClient _httpClient;
    private StreamReader _streamReader;

    public StreamEventListener(string uri, string key)
    {
        _uri = uri;
        _httpClient = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        });

        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("hue-application-key", key);
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/event-stream");
    }

    public async Task<StreamEventListener> StartAsync()
    {
        var streamEventListener = new TaskCompletionSource<StreamEventListener>();

        _streamReader = new StreamReader(await _httpClient.GetStreamAsync(_uri));

        _ = Task.Factory.StartNew(async () =>
        {
            while (!_streamReader.EndOfStream)
            {
                var line = await _streamReader.ReadLineAsync();

                if (line.StartsWith("data:"))
                {
                    line = line
                        .TrimStart("data:".ToCharArray());
                    var statusFromLine = Message.FromJson(line).First();
                    if (statusFromLine != null)
                    {
                        _action?.Invoke(statusFromLine);
                    }
                }
            }
        });

        streamEventListener.SetResult(this);

        return await streamEventListener.Task;
    }

    public StreamEventListener OnEvent(Action<Message> action)
    {
        _action = action;
        return this;
    }

    public void Stop()
    {
        _streamReader.Close();

        _httpClient.Dispose();
        _streamReader.Dispose();
    }
}