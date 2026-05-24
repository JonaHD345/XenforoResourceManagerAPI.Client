using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Provides access to author endpoints.
  /// </summary>
  public sealed class AuthorClient
  {
    private readonly XenforoResourceManagerApiTransport _transport;

    internal AuthorClient(XenforoResourceManagerApiTransport transport)
    {
      _transport = transport ?? throw new ArgumentNullException(nameof(transport));
    }

    /// <summary>
    /// Gets an author by user id.
    /// </summary>
    /// <param name="authorId">The author user id.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The author returned by the API.</returns>
    public Task<Author> GetAsync(int authorId, CancellationToken cancellationToken = default)
    {
      XenforoResourceManagerApiTransport.ValidatePositiveNumber(authorId, nameof(authorId));

      return _transport.SendAsync<Author>(
        HttpMethod.Get,
        "getAuthor",
        cancellationToken,
        XenforoResourceManagerApiTransport.CreateParameter("id", XenforoResourceManagerApiTransport.FormatNumber(authorId)));
    }

    /// <summary>
    /// Finds an author by exact username.
    /// </summary>
    /// <param name="name">The exact username to find.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The author returned by the API.</returns>
    public Task<Author> FindAsync(string name, CancellationToken cancellationToken = default)
    {
      XenforoResourceManagerApiTransport.ValidateRequiredString(name, nameof(name));

      return _transport.SendAsync<Author>(
        HttpMethod.Get,
        "findAuthor",
        cancellationToken,
        XenforoResourceManagerApiTransport.CreateParameter("name", name));
    }
  }
}
