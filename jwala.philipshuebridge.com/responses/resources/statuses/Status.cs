using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace jwala.philipshuebridge.com.responses.resources.statuses
{
    public partial class Status
    {
        [JsonProperty("errors")]
        public object[] Errors { get; set; }

        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

    

    public partial class Status
    {
        public static Status FromJson(string json) => JsonConvert.DeserializeObject<Status>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Status self) => JsonConvert.SerializeObject(self, Converter.Settings);
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
}
