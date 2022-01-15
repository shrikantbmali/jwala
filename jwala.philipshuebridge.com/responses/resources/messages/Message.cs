﻿using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace jwala.philipshuebridge.com.responses.resources.messages;

public partial class Message
{
    [JsonProperty("creationtime")]
    public DateTimeOffset Creationtime { get; set; }

    [JsonProperty("data")]
    public Datum[] Data { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}

public partial class Message
{
    public static Message[] FromJson(string json) => JsonConvert.DeserializeObject<Message[]>(json, Converter.Settings);
}

public static class Serialize
{
    public static string ToJson(this Message[] self) => JsonConvert.SerializeObject(self, Converter.Settings);
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