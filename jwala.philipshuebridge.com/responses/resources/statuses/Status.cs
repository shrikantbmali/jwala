using System;
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

    public partial class Datum
    {
        [JsonProperty("alert")]
        public Alert Alert { get; set; }

        [JsonProperty("color")]
        public Color Color { get; set; }

        [JsonProperty("color_temperature")]
        public ColorTemperature ColorTemperature { get; set; }

        [JsonProperty("dimming")]
        public Dimming Dimming { get; set; }

        [JsonProperty("dynamics")]
        public Dynamics Dynamics { get; set; }

        [JsonProperty("effects")]
        public Effects Effects { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("id_v1")]
        public string IdV1 { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("on")]
        public On On { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Alert
    {
        [JsonProperty("action_values")]
        public string[] ActionValues { get; set; }
    }

    public partial class Color
    {
        [JsonProperty("gamut")]
        public Gamut Gamut { get; set; }

        [JsonProperty("gamut_type")]
        public string GamutType { get; set; }

        [JsonProperty("xy")]
        public Xy Xy { get; set; }
    }

    public partial class Gamut
    {
        [JsonProperty("blue")]
        public Xy Blue { get; set; }

        [JsonProperty("green")]
        public Xy Green { get; set; }

        [JsonProperty("red")]
        public Xy Red { get; set; }
    }

    public partial class Xy
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }

    public partial class ColorTemperature
    {
        [JsonProperty("mirek")]
        public object Mirek { get; set; }

        [JsonProperty("mirek_schema")]
        public MirekSchema MirekSchema { get; set; }

        [JsonProperty("mirek_valid")]
        public bool MirekValid { get; set; }
    }

    public partial class MirekSchema
    {
        [JsonProperty("mirek_maximum")]
        public long MirekMaximum { get; set; }

        [JsonProperty("mirek_minimum")]
        public long MirekMinimum { get; set; }
    }

    public partial class Dimming
    {
        [JsonProperty("brightness")]
        public double Brightness { get; set; }

        [JsonProperty("min_dim_level")]
        public double MinDimLevel { get; set; }
    }

    public partial class Dynamics
    {
        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("speed_valid")]
        public bool SpeedValid { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("status_values")]
        public string[] StatusValues { get; set; }
    }

    public partial class Effects
    {
        [JsonProperty("effect_values")]
        public string[] EffectValues { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("status_values")]
        public string[] StatusValues { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("archetype")]
        public string Archetype { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class On
    {
        [JsonProperty("on")]
        public bool OnOn { get; set; }
    }

    public partial class Owner
    {
        [JsonProperty("rid")]
        public string Rid { get; set; }

        [JsonProperty("rtype")]
        public string Rtype { get; set; }
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
