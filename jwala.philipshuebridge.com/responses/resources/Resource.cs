using System;
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

public partial class Datum
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("id_v1")]
    public string IdV1 { get; set; }

    [JsonProperty("metadata")]
    public Metadata Metadata { get; set; }

    [JsonProperty("product_data")]
    public ProductData ProductData { get; set; }

    [JsonProperty("services")]
    public Service[] Services { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}

public partial class Service
{
    [JsonProperty("rid")]
    public string Rid { get; set; }

    [JsonProperty("rtype")]
    public string Rtype { get; set; }
}

public partial class ProductData
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

public partial class Metadata
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
