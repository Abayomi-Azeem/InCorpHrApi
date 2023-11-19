﻿using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InCorpApp.Api.Helpers
{
    public class JsonDateOnlyConverter : JsonConverter<DateOnly>
    {
        private const string format = "yyyy-MM-dd";
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var readerString = reader.GetString();
            var val = DateOnly.ParseExact(readerString, format, CultureInfo.InvariantCulture);
            return val;
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(format, CultureInfo.InvariantCulture));
        }
    }
}
