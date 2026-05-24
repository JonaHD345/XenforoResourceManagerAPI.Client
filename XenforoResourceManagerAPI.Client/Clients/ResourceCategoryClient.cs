using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Provides access to resource category endpoints.
  /// </summary>
  public sealed class ResourceCategoryClient
  {
    private readonly XenforoResourceManagerApiTransport _transport;

    internal ResourceCategoryClient(XenforoResourceManagerApiTransport transport)
    {
      _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

    /// <summary>
    /// Gets all available resource categories.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The resource categories returned by the API.</returns>
    public Task<List<ResourceCategory>> ListAsync(CancellationToken cancellationToken = default)
    {
      return _transport.SendAsync<List<ResourceCategory>>(
        HttpMethod.Get,
        "listResourceCategories",
        cancellationToken);
    }
  }
}
