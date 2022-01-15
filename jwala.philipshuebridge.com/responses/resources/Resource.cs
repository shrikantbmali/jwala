using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace jwala.philipshuebridge.com.responses.resources;

public partial class Resource
{
    [JsonProperty("errors")]
    public object[] Errors { get; set; }

    [JsonProperty("data")]
    public Datum[] Data { get; set; }
}

public class Datum
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
    [JsonProperty("product_data")]
    public ProductData ProductData { get; set; }

    [JsonProperty("services")]
    public Service[] Services { get; set; }
}

public class Alert
{
    [JsonProperty("action_values")]
    public string[] ActionValues { get; set; }
}

public class Color
{
    [JsonProperty("gamut")]
    public Gamut Gamut { get; set; }

    [JsonProperty("gamut_type")]
    public string GamutType { get; set; }

    [JsonProperty("xy")]
    public Xy Xy { get; set; }
}

public class Gamut
{
    [JsonProperty("blue")]
    public Xy Blue { get; set; }

    [JsonProperty("green")]
    public Xy Green { get; set; }

    [JsonProperty("red")]
    public Xy Red { get; set; }
}

public class Xy
{
    [JsonProperty("x")]
    public double X { get; set; }

    [JsonProperty("y")]
    public double Y { get; set; }
}

public class ColorTemperature
{
    [JsonProperty("mirek")]
    public object Mirek { get; set; }

    [JsonProperty("mirek_schema")]
    public MirekSchema MirekSchema { get; set; }

    [JsonProperty("mirek_valid")]
    public bool MirekValid { get; set; }
}

public class MirekSchema
{
    [JsonProperty("mirek_maximum")]
    public long MirekMaximum { get; set; }

    [JsonProperty("mirek_minimum")]
    public long MirekMinimum { get; set; }
}

public class Dimming
{
    [JsonProperty("brightness")]
    public double Brightness { get; set; }

    [JsonProperty("min_dim_level")]
    public double MinDimLevel { get; set; }
}

public class Dynamics
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

public class Effects
{
    [JsonProperty("effect_values")]
    public string[] EffectValues { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("status_values")]
    public string[] StatusValues { get; set; }
}

public class On
{
    [JsonProperty("on")]
    public bool OnOn { get; set; }
}

public class Owner
{
    [JsonProperty("rid")]
    public string Rid { get; set; }

    [JsonProperty("rtype")]
    public string Rtype { get; set; }
}

public class Service
{
    [JsonProperty("rid")]
    public string Rid { get; set; }

    [JsonProperty("rtype")]
    public string Rtype { get; set; }
}

public class ProductData
{
    [JsonProperty("certified")]
    public bool Certified { get; set; }

    [JsonProperty("manufacturer_name")]
    public string ManufacturerName { get; set; }

    [JsonProperty("model_id")]
    public string ModelId { get; set; }

    [JsonProperty("product_archetype")]
    public string ProductArchetype { get; set; }

    [JsonProperty("product_name")]
    public string ProductName { get; set; }

    [JsonProperty("software_version")]
    public string SoftwareVersion { get; set; }
}

public class Metadata
{
    [JsonProperty("archetype")]
    public string Archetype { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}

public partial class Resource
{
    public static Resource FromJson(string json) =>
        JsonConvert.DeserializeObject<Resource>(json, Converter.Settings);
}

public static class Serialize
{
    public static string ToJson(this Resource self) =>
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

