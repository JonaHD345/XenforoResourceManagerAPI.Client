using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Represents the author information embedded in a resource response.
  /// </summary>
  public sealed class ResourceAuthor
  {
    /// <summary>
    /// Gets or sets the author user id.
    /// </summary>
    [JsonProperty("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    [JsonProperty("username")]
    public string? Username { get; set; }
  }
}
