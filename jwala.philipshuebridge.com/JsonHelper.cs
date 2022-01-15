using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RestSharp;

namespace jwala.philipshuebridge.com;

public static class JsonHelper
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters = {
            new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
        },
        NullValueHandling = NullValueHandling.Ignore
    };

    public static T FromJson<T>(this string json) => JsonConvert.DeserializeObject<T>(json, Settings);

    public static string ToJson<T>(this T anyObject) => JsonConvert.SerializeObject(anyObject, Settings);

    public static RestRequest AddBodyAsJson<T>(this RestRequest restRequest, T anyObject) =>
        restRequest.AddBodyAsJson(anyObject.ToJson());

    public static RestRequest AddBodyAsJson(this RestRequest restRequest, string json) =>
        restRequest.AddBody(json, "applciation/json");
}