using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Represents resource review statistics.
  /// </summary>
  public sealed class ResourceReviewStats
  {
    /// <summary>
    /// Gets or sets the unique review count.
    /// </summary>
    [JsonProperty("unique")]
    public int? Unique { get; set; }

    /// <summary>
    /// Gets or sets the total review count.
    /// </summary>
    [JsonProperty("total")]
    public int? Total { get; set; }
  }
}
