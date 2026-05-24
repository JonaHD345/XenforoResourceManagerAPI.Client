using System;
using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Represents a resource update.
  /// </summary>
  public sealed class ResourceUpdate
  {
    /// <summary>
    /// Gets or sets the resource update id.
    /// </summary>
    [JsonProperty("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the resource id.
    /// </summary>
    [JsonProperty("resource_id")]
    public int? ResourceId { get; set; }

    /// <summary>
    /// Gets or sets the version id.
    /// </summary>
    [JsonProperty("version_id")]
    public int? VersionId { get; set; }

    /// <summary>
    /// Gets or sets the resource version.
    /// </summary>
    [JsonProperty("resource_version")]
    public string? ResourceVersion { get; set; }

    /// <summary>
    /// Gets or sets the download count.
    /// </summary>
    [JsonProperty("download_count")]
    public int? DownloadCount { get; set; }

    /// <summary>
    /// Gets or sets the post date.
    /// </summary>
    [JsonProperty("post_date")]
    public DateTimeOffset? PostDate { get; set; }

    /// <summary>
    /// Gets or sets the update title.
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the update message.
    /// </summary>
    [JsonProperty("message")]
    public string? Message { get; set; }
  }
}
