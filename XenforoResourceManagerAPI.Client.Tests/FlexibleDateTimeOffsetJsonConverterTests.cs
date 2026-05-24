using System.Globalization;
using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client.Tests
{
  public sealed class FlexibleDateTimeOffsetJsonConverterTests
  {
    [Fact]
    public void CanConvert_WithDateTimeOffset_ReturnsTrue()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();

      // Act
      var canConvert = converter.CanConvert(typeof(DateTimeOffset));

      // Assert
      Assert.True(canConvert);
    }

    [Fact]
    public void CanConvert_WithNullableDateTimeOffset_ReturnsTrue()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();

      // Act
      var canConvert = converter.CanConvert(typeof(DateTimeOffset?));

      // Assert
      Assert.True(canConvert);
    }

    [Fact]
    public void CanConvert_WithOtherType_ReturnsFalse()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();

      // Act
      var canConvert = converter.CanConvert(typeof(string));

      // Assert
      Assert.False(canConvert);
    }

    [Fact]
    public void ReadJson_WithStringValue_ReturnsParsedDateTimeOffset()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();
      using var reader = CreateReader("\"2026-05-24T12:30:00Z\"", DateParseHandling.None);

      // Act
      var value = (DateTimeOffset?)converter.ReadJson(reader, typeof(DateTimeOffset), null, JsonSerializer.CreateDefault());

      // Assert
      Assert.Equal(new DateTimeOffset(2026, 5, 24, 12, 30, 0, TimeSpan.Zero), value);
    }

    [Fact]
    public void ReadJson_WithDateToken_ReturnsParsedDateTimeOffset()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();
      using var reader = CreateReader("\"2026-05-24T12:30:00Z\"");

      // Act
      var value = (DateTimeOffset?)converter.ReadJson(reader, typeof(DateTimeOffset), null, JsonSerializer.CreateDefault());

      // Assert
      Assert.Equal(new DateTimeOffset(2026, 5, 24, 12, 30, 0, TimeSpan.Zero), value);
    }

    [Fact]
    public void ReadJson_WithUnixTimestamp_ReturnsDateTimeOffset()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();
      using var reader = CreateReader("1748089800");

      // Act
      var value = (DateTimeOffset?)converter.ReadJson(reader, typeof(DateTimeOffset), null, JsonSerializer.CreateDefault());

      // Assert
      Assert.Equal(DateTimeOffset.FromUnixTimeSeconds(1748089800), value);
    }

    [Fact]
    public void ReadJson_WithNullForNullable_ReturnsNull()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();
      using var reader = CreateReader("null");

      // Act
      var value = converter.ReadJson(reader, typeof(DateTimeOffset?), null, JsonSerializer.CreateDefault());

      // Assert
      Assert.Null(value);
    }

    [Fact]
    public void ReadJson_WithEmptyStringForNullable_ReturnsNull()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();
      using var reader = CreateReader("\"\"");

      // Act
      var value = converter.ReadJson(reader, typeof(DateTimeOffset?), null, JsonSerializer.CreateDefault());

      // Assert
      Assert.Null(value);
    }

    [Fact]
    public void ReadJson_WithNullForRequiredType_ThrowsJsonSerializationException()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();
      using var reader = CreateReader("null");

      // Act
      var exception = Assert.Throws<JsonSerializationException>(() => converter.ReadJson(reader, typeof(DateTimeOffset), null, JsonSerializer.CreateDefault()));

      // Assert
      Assert.Equal("The JSON value could not be converted to a DateTimeOffset.", exception.Message);
    }

    [Fact]
    public void ReadJson_WithInvalidString_ThrowsJsonSerializationException()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();
      using var reader = CreateReader("\"not-a-date\"");

      // Act
      var exception = Assert.Throws<JsonSerializationException>(() => converter.ReadJson(reader, typeof(DateTimeOffset), null, JsonSerializer.CreateDefault()));

      // Assert
      Assert.Equal("The JSON value could not be converted to a DateTimeOffset.", exception.Message);
    }

    [Fact]
    public void WriteJson_WithNull_WritesNull()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();

      // Act
      var json = WriteJson(converter, null);

      // Assert
      Assert.Equal("null", json);
    }

    [Fact]
    public void WriteJson_WithDateTimeOffset_WritesUniversalIsoString()
    {
      // Arrange
      var converter = new FlexibleDateTimeOffsetJsonConverter();
      var value = new DateTimeOffset(2026, 5, 24, 14, 30, 0, TimeSpan.FromHours(2));

      // Act
      var json = WriteJson(converter, value);

      // Assert
      Assert.Equal("\"2026-05-24T12:30:00.0000000+00:00\"", json);
    }

    private static JsonTextReader CreateReader(string json, DateParseHandling dateParseHandling = DateParseHandling.DateTime)
    {
      var reader = new JsonTextReader(new StringReader(json))
      {
        Culture = CultureInfo.InvariantCulture,
        DateParseHandling = dateParseHandling
      };

      reader.Read();

      return reader;
    }

    private static string WriteJson(JsonConverter converter, object? value)
    {
      using var stringWriter = new StringWriter(CultureInfo.InvariantCulture);
      using var writer = new JsonTextWriter(stringWriter);

      converter.WriteJson(writer, value, JsonSerializer.CreateDefault());
      writer.Flush();

      return stringWriter.ToString();
    }
  }
}
