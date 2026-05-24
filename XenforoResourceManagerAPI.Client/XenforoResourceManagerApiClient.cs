using System;
using System.Net.Http;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Provides strongly typed access to the XenForo Resource Manager API exposed by SpigotMC.
  /// </summary>
  public sealed class XenforoResourceManagerApiClient : IDisposable
  {
    private readonly XenforoResourceManagerApiTransport _transport;

    /// <summary>
    /// Initializes a new instance of the <see cref="XenforoResourceManagerApiClient"/> class with a new <see cref="HttpClient"/>.
    /// </summary>
    public XenforoResourceManagerApiClient()
      : this(new HttpClient(), new XenforoResourceManagerApiClientOptions(), true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XenforoResourceManagerApiClient"/> class with custom options.
    /// </summary>
    /// <param name="options">The client options.</param>
    public XenforoResourceManagerApiClient(XenforoResourceManagerApiClientOptions options)
      : this(new HttpClient(), options, true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XenforoResourceManagerApiClient"/> class with an existing <see cref="HttpClient"/>.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to send API requests.</param>
    /// <param name="options">The optional client options.</param>
    public XenforoResourceManagerApiClient(HttpClient httpClient, XenforoResourceManagerApiClientOptions? options = null)
      : this(httpClient, options ?? new XenforoResourceManagerApiClientOptions(), false)
    {
    }

    private XenforoResourceManagerApiClient(HttpClient httpClient, XenforoResourceManagerApiClientOptions options, bool disposeHttpClient)
    {
      _transport = new XenforoResourceManagerApiTransport(httpClient, options, disposeHttpClient);

      Resources = new ResourceClient(_transport);
      ResourceCategories = new ResourceCategoryClient(_transport);
      ResourceUpdates = new ResourceUpdateClient(_transport);
      Authors = new AuthorClient(_transport);
    }

    /// <summary>
    /// Gets the resource API.
    /// </summary>
    public ResourceClient Resources { get; }

    /// <summary>
    /// Gets the resource category API.
    /// </summary>
    public ResourceCategoryClient ResourceCategories { get; }

    /// <summary>
    /// Gets the resource update API.
    /// </summary>
    public ResourceUpdateClient ResourceUpdates { get; }

    /// <summary>
    /// Gets the author API.
    /// </summary>
    public AuthorClient Authors { get; }

    /// <summary>
    /// Releases resources owned by the client.
    /// </summary>
    public void Dispose()
    {
      _transport.Dispose();
    }
  }
}
