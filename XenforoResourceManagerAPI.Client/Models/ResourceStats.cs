using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Represents resource statistics.
  /// </summary>
  public sealed class ResourceStats
  {
    /// <summary>
    /// Gets or sets the download count.
    /// </summary>
    [JsonProperty("downloads")]
    public int? Downloads { get; set; }

    /// <summary>
    /// Gets or sets the update count.
    /// </summary>
    [JsonProperty("updates")]
    public int? Updates { get; set; }

    /// <summary>
    /// Gets or sets the review statistics.
    /// </summary>
    [JsonProperty("reviews")]
    public ResourceReviewStats? Reviews { get; set; }

    /// <summary>
    /// Gets or sets the average rating.
    /// </summary>
    [JsonProperty("rating")]
    public decimal? Rating { get; set; }
  }
}
