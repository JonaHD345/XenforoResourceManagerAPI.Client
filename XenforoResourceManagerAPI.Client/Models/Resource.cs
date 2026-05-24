using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Represents a XenForo resource.
  /// </summary>
  public sealed class Resource
  {
    /// <summary>
    /// Gets or sets the resource id.
    /// </summary>
    [JsonProperty("id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or sets the resource title.
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets the short resource tag line.
    /// </summary>
    [JsonProperty("tag")]
    public string? Tag { get; set; }

    /// <summary>
    /// Gets or sets the current resource version.
    /// </summary>
    [JsonProperty("current_version")]
    public string? CurrentVersion { get; set; }

    /// <summary>
    /// Gets or sets the resource category.
    /// </summary>
    [JsonProperty("category")]
    public ResourceCategory? Category { get; set; }

    /// <summary>
    /// Gets or sets the native Minecraft version.
    /// </summary>
    [JsonProperty("native_minecraft_version")]
    public string? NativeMinecraftVersion { get; set; }

    /// <summary>
    /// Gets or sets the supported Minecraft versions.
    /// </summary>
    [JsonProperty("supported_minecraft_versions")]
    public List<string>? SupportedMinecraftVersions { get; set; }

    /// <summary>
    /// Gets or sets the resource icon URL.
    /// </summary>
    [JsonProperty("icon_link")]
    public string? IconLink { get; set; }

    /// <summary>
    /// Gets or sets the resource author.
    /// </summary>
    [JsonProperty("author")]
    public ResourceAuthor? Author { get; set; }

    /// <summary>
    /// Gets or sets the premium resource metadata.
    /// </summary>
    [JsonProperty("premium")]
    public ResourcePremium? Premium { get; set; }

    /// <summary>
    /// Gets or sets the resource statistics.
    /// </summary>
    [JsonProperty("stats")]
    public ResourceStats? Stats { get; set; }

    /// <summary>
    /// Gets or sets the first release time.
    /// </summary>
    [JsonProperty("first_release")]
    public DateTimeOffset? FirstRelease { get; set; }

    /// <summary>
    /// Gets or sets the last update time.
    /// </summary>
    [JsonProperty("last_update")]
    public DateTimeOffset? LastUpdate { get; set; }

    /// <summary>
    /// Gets or sets the resource description.
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the source code URL.
    /// </summary>
    [JsonProperty("source_code_url")]
    public string? SourceCodeUrl { get; set; }

    /// <summary>
    /// Gets or sets the donation URL.
    /// </summary>
    [JsonProperty("donate_url")]
    public string? DonateUrl { get; set; }
  }
}
