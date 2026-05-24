using System;
using System.Globalization;
using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  internal sealed class FlexibleDateTimeOffsetJsonConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      var targetType = Nullable.GetUnderlyingType(objectType) ?? objectType;
      return targetType == typeof(DateTimeOffset);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
      var isNullable = Nullable.GetUnderlyingType(objectType) != null;

      if (reader.TokenType == JsonToken.Null)
      {
        if (isNullable)
        {
          return null;
        }

        throw new JsonSerializationException("The JSON value could not be converted to a DateTimeOffset.");
      }

      if (reader.TokenType == JsonToken.String)
      {
        var value = reader.Value?.ToString();

        if (string.IsNullOrWhiteSpace(value) && isNullable)
        {
          return null;
        }

        if (!string.IsNullOrWhiteSpace(value)
          && DateTimeOffset.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsedValue))
        {
          return parsedValue;
        }
      }

      if (reader.TokenType == JsonToken.Integer)
      {
        var unixTimestamp = Convert.ToInt64(reader.Value, CultureInfo.InvariantCulture);
        return DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
      }

      throw new JsonSerializationException("The JSON value could not be converted to a DateTimeOffset.");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
        return;
      }

      var dateTimeOffset = (DateTimeOffset)value;
      writer.WriteValue(dateTimeOffset.ToUniversalTime().ToString("O", CultureInfo.InvariantCulture));
    }
  }
}
