using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Represents a resource author.
  /// </summary>
  public sealed class Author
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

    /// <summary>
    /// Gets or sets the number of resources created by the author.
    /// </summary>
    [JsonProperty("resource_count")]
    public int? ResourceCount { get; set; }

    /// <summary>
    /// Gets or sets the public identities associated with the author.
    /// </summary>
    [JsonProperty("identities")]
    public Dictionary<string, string>? Identities { get; set; }

    /// <summary>
    /// Gets or sets the author avatar URL.
    /// </summary>
    [JsonProperty("avatar")]
    public string? Avatar { get; set; }

    /// <summary>
    /// Gets or sets the last activity time.
    /// </summary>
    [JsonProperty("last_activity")]
    public DateTimeOffset? LastActivity { get; set; }
  }
}
