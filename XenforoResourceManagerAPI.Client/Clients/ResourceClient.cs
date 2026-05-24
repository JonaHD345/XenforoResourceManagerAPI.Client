using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Provides access to resource endpoints.
  /// </summary>
  public sealed class ResourceClient
  {
    private readonly XenforoResourceManagerApiTransport _transport;

    internal ResourceClient(XenforoResourceManagerApiTransport transport)
    {
      _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

    /// <summary>
    /// Gets resources from the API.
    /// </summary>
    /// <param name="category">The optional resource category id used to restrict results.</param>
    /// <param name="page">The optional page number.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The resources returned by the API.</returns>
    public Task<List<Resource>> ListAsync(int? category = null, int? page = null, CancellationToken cancellationToken = default)
    {
      XenforoResourceManagerApiTransport.ValidateOptionalPositiveNumber(category, nameof(category));
      XenforoResourceManagerApiTransport.ValidateOptionalPositiveNumber(page, nameof(page));

      return _transport.SendAsync<List<Resource>>(
        HttpMethod.Get,
        "listResources",
        cancellationToken,
        XenforoResourceManagerApiTransport.CreateParameter("category", XenforoResourceManagerApiTransport.FormatOptionalNumber(category)),
        XenforoResourceManagerApiTransport.CreateParameter("page", XenforoResourceManagerApiTransport.FormatOptionalNumber(page)));
    }

    /// <summary>
    /// Gets a resource by resource id.
    /// </summary>
    /// <param name="resourceId">The resource id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The resource returned by the API.</returns>
    public Task<Resource> GetAsync(int resourceId, CancellationToken cancellationToken = default)
    {
      XenforoResourceManagerApiTransport.ValidatePositiveNumber(resourceId, nameof(resourceId));

      return _transport.SendAsync<Resource>(
        HttpMethod.Get,
        "getResource",
        cancellationToken,
        XenforoResourceManagerApiTransport.CreateParameter("id", XenforoResourceManagerApiTransport.FormatNumber(resourceId)));
    }

    /// <summary>
    /// Gets resources created by an author.
    /// </summary>
    /// <param name="authorId">The author user id.</param>
    /// <param name="page">The optional page number.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The resources returned by the API.</returns>
    public Task<List<Resource>> GetByAuthorAsync(int authorId, int? page = null, CancellationToken cancellationToken = default)
    {
      XenforoResourceManagerApiTransport.ValidatePositiveNumber(authorId, nameof(authorId));
      XenforoResourceManagerApiTransport.ValidateOptionalPositiveNumber(page, nameof(page));

      return _transport.SendAsync<List<Resource>>(
        HttpMethod.Get,
        "getResourcesByAuthor",
        cancellationToken,
        XenforoResourceManagerApiTransport.CreateParameter("id", XenforoResourceManagerApiTransport.FormatNumber(authorId)),
        XenforoResourceManagerApiTransport.CreateParameter("page", XenforoResourceManagerApiTransport.FormatOptionalNumber(page)));
    }
  }
}
