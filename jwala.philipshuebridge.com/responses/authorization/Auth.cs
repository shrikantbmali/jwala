using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace jwala.philipshuebridge.com.responses.authorization;

public partial class Auth
{
    [JsonProperty("success", NullValueHandling = NullValueHandling.Ignore)]
    public Success Success { get; set; }

    [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
    public Error Error { get; set; }
}

public static class Serialize
{
    public static string ToJson(this Auth[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
}

public partial class Auth
{
    public static Auth FromJson(string json) => JsonConvert.DeserializeObject<Auth>(json, Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters = {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
    };
}

public partial class Success
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("clientkey")]
    public string Clientkey { get; set; }
}

public partial class Error
{
    [JsonProperty("type")]
    public long Type { get; set; }

    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}