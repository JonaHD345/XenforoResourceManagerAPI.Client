using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XenforoResourceManagerAPI.Client
{
  internal sealed class SingleOrArrayJsonConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(List<>);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null)
      {
        return null;
      }

      var itemType = objectType.GetGenericArguments()[0];
      var listType = typeof(List<>).MakeGenericType(itemType);
      var list = (IList)Activator.CreateInstance(listType)!;

      var token = JToken.Load(reader);

      if (token.Type == JTokenType.Array)
      {
        foreach (var child in token.Children())
        {
          var item = child.ToObject(itemType, serializer);
          list.Add(item);
        }
      }
      else
      {
        var item = token.ToObject(itemType, serializer);
        if (item != null)
        {
          list.Add(item);
        }
      }

      return list;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      if (value == null)
      {
        writer.WriteNull();
        return;
      }

      var list = (IEnumerable)value;
      writer.WriteStartArray();
      foreach (var item in list)
      {
        serializer.Serialize(writer, item);
      }

      writer.WriteEndArray();
    }
  }
}
