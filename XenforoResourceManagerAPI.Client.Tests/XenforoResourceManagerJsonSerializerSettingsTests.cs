using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client.Tests
{
  public sealed class XenforoResourceManagerJsonSerializerSettingsTests
  {
    [Fact]
    public void Create_WithNullSource_ConfiguresDefaultSettings()
    {
      // Arrange
      JsonSerializerSettings? source = null;

      // Act
      var settings = XenforoResourceManagerJsonSerializerSettings.Create(source);

      // Assert
      Assert.Equal(NullValueHandling.Ignore, settings.NullValueHandling);
      Assert.Contains(settings.Converters, converter => converter is FlexibleDateTimeOffsetJsonConverter);
      Assert.Contains(settings.Converters, converter => converter is SingleOrArrayJsonConverter);
    }

    [Fact]
    public void Create_WithSource_ClonesSourceSettingsAndConverters()
    {
      // Arrange
      var markerConverter = new MarkerJsonConverter();
      var source = new JsonSerializerSettings
      {
        Formatting = Formatting.Indented,
        MissingMemberHandling = MissingMemberHandling.Error,
        NullValueHandling = NullValueHandling.Include
      };
      source.Converters.Add(markerConverter);

      // Act
      var settings = XenforoResourceManagerJsonSerializerSettings.Create(source);

      // Assert
      Assert.NotSame(source, settings);
      Assert.Equal(Formatting.Indented, settings.Formatting);
      Assert.Equal(MissingMemberHandling.Error, settings.MissingMemberHandling);
      Assert.Equal(NullValueHandling.Ignore, settings.NullValueHandling);
      Assert.Contains(markerConverter, settings.Converters);
      Assert.DoesNotContain(source.Converters, converter => converter is FlexibleDateTimeOffsetJsonConverter);
      Assert.DoesNotContain(source.Converters, converter => converter is SingleOrArrayJsonConverter);
    }

    [Fact]
    public void Create_WithExistingDefaultConverters_DoesNotDuplicateConverters()
    {
      // Arrange
      var source = new JsonSerializerSettings();
      source.Converters.Add(new FlexibleDateTimeOffsetJsonConverter());
      source.Converters.Add(new SingleOrArrayJsonConverter());

      // Act
      var settings = XenforoResourceManagerJsonSerializerSettings.Create(source);

      // Assert
      Assert.Equal(1, settings.Converters.Count(converter => converter is FlexibleDateTimeOffsetJsonConverter));
      Assert.Equal(1, settings.Converters.Count(converter => converter is SingleOrArrayJsonConverter));
    }

    private sealed class MarkerJsonConverter : JsonConverter
    {
      public override bool CanConvert(Type objectType)
      {
        return false;
      }

      public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
      {
        throw new NotSupportedException();
      }

      public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
      {
        throw new NotSupportedException();
      }
    }
  }
}
