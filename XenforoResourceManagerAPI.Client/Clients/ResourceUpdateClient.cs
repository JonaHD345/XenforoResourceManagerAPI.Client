using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Provides access to resource update endpoints.
  /// </summary>
  public sealed class ResourceUpdateClient
  {
    private readonly XenforoResourceManagerApiTransport _transport;

    internal ResourceUpdateClient(XenforoResourceManagerApiTransport transport)
    {
      _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

    /// <summary>
    /// Gets a resource update by resource update id.
    /// </summary>
    /// <param name="resourceUpdateId">The resource update id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The resource update returned by the API.</returns>
    public Task<ResourceUpdate> GetAsync(int resourceUpdateId, CancellationToken cancellationToken = default)
    {
      XenforoResourceManagerApiTransport.ValidatePositiveNumber(resourceUpdateId, nameof(resourceUpdateId));

      return _transport.SendAsync<ResourceUpdate>(
        HttpMethod.Get,
        "getResourceUpdate",
        cancellationToken,
        XenforoResourceManagerApiTransport.CreateParameter("id", XenforoResourceManagerApiTransport.FormatNumber(resourceUpdateId)));
    }

    /// <summary>
    /// Gets updates for a resource.
    /// </summary>
    /// <param name="resourceId">The resource id.</param>
    /// <param name="page">The optional page number.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The resource updates returned by the API.</returns>
    public Task<List<ResourceUpdate>> GetByResourceAsync(int resourceId, int? page = null, CancellationToken cancellationToken = default)
    {
      XenforoResourceManagerApiTransport.ValidatePositiveNumber(resourceId, nameof(resourceId));
      XenforoResourceManagerApiTransport.ValidateOptionalPositiveNumber(page, nameof(page));

      return _transport.SendAsync<List<ResourceUpdate>>(
        HttpMethod.Get,
        "getResourceUpdates",
        cancellationToken,
        XenforoResourceManagerApiTransport.CreateParameter("id", XenforoResourceManagerApiTransport.FormatNumber(resourceId)),
        XenforoResourceManagerApiTransport.CreateParameter("page", XenforoResourceManagerApiTransport.FormatOptionalNumber(page)));
    }
  }
}
