using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace jwala.philipshuebridge.com.responses.configurations;

internal partial class ConfigResponse
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("datastoreversion")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Datastoreversion { get; set; }

    [JsonProperty("swversion")]
    [JsonConverter(typeof(ParseStringConverter))]
    public long Swversion { get; set; }

    [JsonProperty("apiversion")]
    public string Apiversion { get; set; }

    [JsonProperty("mac")]
    public string Mac { get; set; }

    [JsonProperty("bridgeid")]
    public string Bridgeid { get; set; }

    [JsonProperty("factorynew")]
    public bool Factorynew { get; set; }

    [JsonProperty("replacesbridgeid")]
    public object Replacesbridgeid { get; set; }

    [JsonProperty("modelid")]
    public string Modelid { get; set; }

    [JsonProperty("starterkitid")]
    public string Starterkitid { get; set; }
}

internal partial class ConfigResponse
{
    public static ConfigResponse FromJson(string json) =>
        JsonConvert.DeserializeObject<ConfigResponse>(json, Converter.Settings);
}

internal static class Serialize
{
    public static string ToJson(this ConfigResponse self) =>
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

internal class ParseStringConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        long l;
        if (Int64.TryParse(value, out l))
        {
            return l;
        }
        throw new Exception("Cannot unmarshal type long");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (long)untypedValue;
        serializer.Serialize(writer, value.ToString());
        return;
    }

    public static readonly ParseStringConverter Singleton = new ParseStringConverter();
}