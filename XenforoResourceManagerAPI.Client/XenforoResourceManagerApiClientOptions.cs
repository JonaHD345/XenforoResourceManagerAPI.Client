using System;
using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Configures the XenForo Resource Manager API client.
  /// </summary>
  public sealed class XenforoResourceManagerApiClientOptions
  {
    /// <summary>
    /// Gets the default production API base address.
    /// </summary>
    public static Uri DefaultBaseAddress { get; } = new Uri("https://api.spigotmc.org/simple/0.2/");

    /// <summary>
    /// Gets or sets the API base address used for requests.
    /// </summary>
    public Uri BaseAddress { get; set; } = DefaultBaseAddress;

    /// <summary>
    /// Gets or sets custom JSON serializer settings.
    /// </summary>
    public JsonSerializerSettings? JsonSerializerSettings { get; set; }
  }
}
