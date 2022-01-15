using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace jwala.philipshuebridge.com.responses.discovery;

internal partial class DiscoveryResponse
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("internalipaddress")]
    public string Internalipaddress { get; set; }

    [JsonProperty("port")]
    public long Port { get; set; }
}

internal partial class DiscoveryResponse
{
    public static DiscoveryResponse[] FromJson(string json) =>
        JsonConvert.DeserializeObject<DiscoveryResponse[]>(json, Converter.Settings);
}

internal static class Serialize
{
    public static string ToJson(this DiscoveryResponse[] self) =>
        JsonConvert.SerializeObject(self, Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
        {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };
}