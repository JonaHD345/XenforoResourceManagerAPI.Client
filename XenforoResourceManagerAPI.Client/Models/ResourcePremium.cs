using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Represents premium metadata for a resource.
  /// </summary>
  public sealed class ResourcePremium
  {
    /// <summary>
    /// Gets or sets the premium price.
    /// </summary>
    [JsonProperty("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Gets or sets the premium currency.
    /// </summary>
    [JsonProperty("currency")]
    public string? Currency { get; set; }
  }
}
