using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace XenforoResourceManagerAPI.Client.Tests
{
  public sealed class TestHttpMessageHandler : HttpMessageHandler
  {
    private readonly Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> _send;

    public TestHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, HttpResponseMessage> send)
    {
      _send = send ?? throw new ArgumentNullException(nameof(send));
    }

    public List<RequestSnapshot> Requests { get; } = new List<RequestSnapshot>();

    public List<CancellationToken> CancellationTokens { get; } = new List<CancellationToken>();

    public bool IsDisposed { get; private set; }

    public static HttpResponseMessage CreateJsonResponse(string content, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
      return new HttpResponseMessage(statusCode)
      {
        Content = new StringContent(content, Encoding.UTF8, "application/json")
      };
    }

    public static HttpResponseMessage CreateTextResponse(string content, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
      return new HttpResponseMessage(statusCode)
      {
        Content = new StringContent(content, Encoding.UTF8, "text/plain")
      };
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
      Requests.Add(RequestSnapshot.From(request));
      CancellationTokens.Add(cancellationToken);

      return Task.FromResult(_send(request, cancellationToken));
    }

    protected override void Dispose(bool disposing)
    {
      IsDisposed = true;

      base.Dispose(disposing);
    }
  }

  public sealed class RequestSnapshot
  {
    public HttpMethod? Method { get; private set; }

    public Uri? RequestUri { get; private set; }

    public string? Accept { get; private set; }

    public static RequestSnapshot From(HttpRequestMessage request)
    {
      return new RequestSnapshot
      {
        Method = request.Method,
        RequestUri = request.RequestUri,
        Accept = string.Join(",", request.Headers.Accept.Select(FormatHeader))
      };
    }

    private static string FormatHeader(MediaTypeWithQualityHeaderValue header)
    {
      return header.MediaType ?? string.Empty;
    }
  }
}
