using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Represents a resource category.
  /// </summary>
  public sealed class ResourceCategory
  {
    /// <summary>
    /// Gets or sets the resource category id.
    /// </summary>
    [JsonProperty("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the category title.
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the category description.
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }
  }
}
