using System.Globalization;
using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client.Tests
{
  public sealed class SingleOrArrayJsonConverterTests
  {
    [Fact]
    public void CanConvert_WithListType_ReturnsTrue()
    {
      // Arrange
      var converter = new SingleOrArrayJsonConverter();

      // Act
      var canConvert = converter.CanConvert(typeof(List<string>));

      // Assert
      Assert.True(canConvert);
    }

    [Theory]
    [InlineData(typeof(string[]))]
    [InlineData(typeof(string))]
    public void CanConvert_WithOtherType_ReturnsFalse(Type objectType)
    {
      // Arrange
      var converter = new SingleOrArrayJsonConverter();

      // Act
      var canConvert = converter.CanConvert(objectType);

      // Assert
      Assert.False(canConvert);
    }

    [Fact]
    public void ReadJson_WithNull_ReturnsNull()
    {
      // Arrange
      var converter = new SingleOrArrayJsonConverter();
      using var reader = CreateReader("null");

      // Act
      var value = converter.ReadJson(reader, typeof(List<string>), null, JsonSerializer.CreateDefault());

      // Assert
      Assert.Null(value);
    }

    [Fact]
    public void ReadJson_WithArray_ReturnsList()
    {
      // Arrange
      var converter = new SingleOrArrayJsonConverter();
      using var reader = CreateReader("[\"one\",\"two\"]");

      // Act
      var value = (List<string>?)converter.ReadJson(reader, typeof(List<string>), null, JsonSerializer.CreateDefault());

      // Assert
      Assert.NotNull(value);
      Assert.Equal(new[] { "one", "two" }, value);
    }

    [Fact]
    public void ReadJson_WithSingleValue_ReturnsOneItemList()
    {
      // Arrange
      var converter = new SingleOrArrayJsonConverter();
      using var reader = CreateReader("\"one\"");

      // Act
      var value = (List<string>?)converter.ReadJson(reader, typeof(List<string>), null, JsonSerializer.CreateDefault());

      // Assert
      Assert.NotNull(value);
      var item = Assert.Single(value);
      Assert.Equal("one", item);
    }

    [Fact]
    public void WriteJson_WithNull_WritesNull()
    {
      // Arrange
      var converter = new SingleOrArrayJsonConverter();

      // Act
      var json = WriteJson(converter, null);

      // Assert
      Assert.Equal("null", json);
    }

    [Fact]
    public void WriteJson_WithList_WritesArray()
    {
      // Arrange
      var converter = new SingleOrArrayJsonConverter();
      var value = new List<string> { "one", "two" };

      // Act
      var json = WriteJson(converter, value);

      // Assert
      Assert.Equal("[\"one\",\"two\"]", json);
    }

    private static JsonTextReader CreateReader(string json)
    {
      var reader = new JsonTextReader(new StringReader(json))
      {
        Culture = CultureInfo.InvariantCulture
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
