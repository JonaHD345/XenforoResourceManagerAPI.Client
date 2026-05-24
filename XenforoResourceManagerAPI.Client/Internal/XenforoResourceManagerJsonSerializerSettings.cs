using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  internal static class XenforoResourceManagerJsonSerializerSettings
  {
    public static JsonSerializerSettings Create(JsonSerializerSettings? source)
    {
      var settings = source == null
        ? new JsonSerializerSettings()
        : Clone(source);

      settings.NullValueHandling = NullValueHandling.Ignore;

      AddConverterIfMissing(settings, new FlexibleDateTimeOffsetJsonConverter());
      AddConverterIfMissing(settings, new SingleOrArrayJsonConverter());

      return settings;
    }

    private static JsonSerializerSettings Clone(JsonSerializerSettings source)
    {
      var clone = new JsonSerializerSettings
      {
        Context = source.Context,
        Culture = source.Culture,
        ContractResolver = source.ContractResolver,
        ConstructorHandling = source.ConstructorHandling,
        CheckAdditionalContent = source.CheckAdditionalContent,
        DateFormatHandling = source.DateFormatHandling,
        DateFormatString = source.DateFormatString,
        DateParseHandling = source.DateParseHandling,
        DateTimeZoneHandling = source.DateTimeZoneHandling,
        DefaultValueHandling = source.DefaultValueHandling,
        EqualityComparer = source.EqualityComparer,
        FloatFormatHandling = source.FloatFormatHandling,
        FloatParseHandling = source.FloatParseHandling,
        Formatting = source.Formatting,
        MaxDepth = source.MaxDepth,
        MetadataPropertyHandling = source.MetadataPropertyHandling,
        MissingMemberHandling = source.MissingMemberHandling,
        NullValueHandling = source.NullValueHandling,
        ObjectCreationHandling = source.ObjectCreationHandling,
        PreserveReferencesHandling = source.PreserveReferencesHandling,
        ReferenceLoopHandling = source.ReferenceLoopHandling,
        ReferenceResolverProvider = source.ReferenceResolverProvider,
        SerializationBinder = source.SerializationBinder,
        StringEscapeHandling = source.StringEscapeHandling,
        TypeNameAssemblyFormatHandling = source.TypeNameAssemblyFormatHandling,
        TypeNameHandling = source.TypeNameHandling
      };

      if (source.Converters != null)
      {
        foreach (var converter in source.Converters)
        {
          clone.Converters.Add(converter);
        }
      }

      return clone;
    }

    private static void AddConverterIfMissing(JsonSerializerSettings settings, JsonConverter converter)
    {
      foreach (var existingConverter in settings.Converters)
      {
        if (existingConverter.GetType() == converter.GetType())
        {
          return;
        }
      }

      settings.Converters.Add(converter);
    }
  }
}
