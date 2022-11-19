using System.Text.Json;
using LanguageExtCollectionsJson.Converters;

namespace LanguageExtCollectionsJsonSpecification;

public static class SystemTextJsonOptions
{
  public static JsonSerializerOptions WithLanguageExtExtensions()
  {
    var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.General);
    jsonSerializerOptions.Converters.Add(new SeqConverter());
    jsonSerializerOptions.Converters.Add(new ArrConverter());
    return jsonSerializerOptions;
  }
}