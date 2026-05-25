using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XenforoResourceManagerAPI.Client
{
  internal sealed class XenforoResourceManagerApiTransport : IDisposable
  {
    private const string DefaultUserAgentProductName = "XenforoResourceManagerAPI.Client";
    private const string DefaultUserAgentProductVersion = "1.0.1";
    private const string DefaultUserAgentComment = "(+https://github.com/JonaHD345/XenforoResourceManagerAPI.NET)";
    private const string EndpointPath = "index.php";

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerSettings _jsonSerializerSettings;
    private readonly bool _disposeHttpClient;
    private bool _disposed;

    internal XenforoResourceManagerApiTransport(HttpClient httpClient, XenforoResourceManagerApiClientOptions options, bool disposeHttpClient)
    {
      if (httpClient == null)
      {
        throw new ArgumentNullException(nameof(httpClient));
      }

      if (options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      _httpClient = httpClient;
      _disposeHttpClient = disposeHttpClient;
      _jsonSerializerSettings = XenforoResourceManagerJsonSerializerSettings.Create(options.JsonSerializerSettings);

      if (_httpClient.BaseAddress == null)
      {
        _httpClient.BaseAddress = EnsureTrailingSlash(options.BaseAddress);
      }
    }

    public void Dispose()
    {
      if (_disposed)
      {
        return;
      }

      if (_disposeHttpClient)
      {
        _httpClient.Dispose();
      }

      _disposed = true;
    }

    internal async Task<TResponse> SendAsync<TResponse>(
      HttpMethod method,
      string action,
      CancellationToken cancellationToken,
      params KeyValuePair<string, string?>[] parameters)
    {
      EnsureNotDisposed();
      ValidateRequiredString(action, nameof(action));

      var requestPath = BuildRequestPath(action, parameters);

      using (var request = new HttpRequestMessage(method, requestPath))
      {
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        AddDefaultUserAgent(request);

        using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
        {
          var responseContent = response.Content == null
            ? null
            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);

          if (!response.IsSuccessStatusCode)
          {
            throw new XenforoResourceManagerApiException(
              $"The XenForo Resource Manager API returned HTTP {(int)response.StatusCode} ({response.ReasonPhrase}).",
              response.StatusCode,
              responseContent);
          }

          if (string.IsNullOrWhiteSpace(responseContent))
          {
            throw new XenforoResourceManagerApiException("The XenForo Resource Manager API returned an empty response.");
          }

          try
          {
            var result = JsonConvert.DeserializeObject<TResponse>(responseContent, _jsonSerializerSettings);

            if (result == null)
            {
              throw new XenforoResourceManagerApiException("The XenForo Resource Manager API response could not be deserialized.");
            }

            return result;
          }
          catch (JsonException exception)
          {
            throw new XenforoResourceManagerApiException("The XenForo Resource Manager API response could not be deserialized.", exception);
          }
        }
      }
    }

    private void AddDefaultUserAgent(HttpRequestMessage request)
    {
      if (_httpClient.DefaultRequestHeaders.UserAgent.Any())
      {
        return;
      }

      request.Headers.UserAgent.Add(new ProductInfoHeaderValue(DefaultUserAgentProductName, DefaultUserAgentProductVersion));
      request.Headers.UserAgent.Add(new ProductInfoHeaderValue(DefaultUserAgentComment));
    }

    internal static KeyValuePair<string, string?> CreateParameter(string name, string? value)
    {
      return new KeyValuePair<string, string?>(name, value);
    }

    internal static void ValidatePositiveNumber(int value, string parameterName)
    {
      if (value <= 0)
      {
        throw new ArgumentOutOfRangeException(parameterName, value, "The value must be greater than zero.");
      }
    }

    internal static void ValidateOptionalPositiveNumber(int? value, string parameterName)
    {
      if (value.HasValue)
      {
        ValidatePositiveNumber(value.Value, parameterName);
      }
    }

    internal static void ValidateRequiredString(string? value, string parameterName)
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        throw new ArgumentException("The value is required.", parameterName);
      }
    }

    internal static string FormatNumber(int value)
    {
      return value.ToString(CultureInfo.InvariantCulture);
    }

    internal static string? FormatOptionalNumber(int? value)
    {
      return value.HasValue
        ? FormatNumber(value.Value)
        : null;
    }

    private static string BuildRequestPath(string action, IEnumerable<KeyValuePair<string, string?>> parameters)
    {
      var builder = new StringBuilder(EndpointPath);
      builder.Append('?');
      AppendQueryParameter(builder, "action", action, true);

      foreach (var parameter in parameters)
      {
        if (parameter.Value == null)
        {
          continue;
        }

        AppendQueryParameter(builder, parameter.Key, parameter.Value, false);
      }

      return builder.ToString();
    }

    private static void AppendQueryParameter(StringBuilder builder, string name, string value, bool first)
    {
      if (!first)
      {
        builder.Append('&');
      }

      builder.Append(Uri.EscapeDataString(name));
      builder.Append('=');
      builder.Append(Uri.EscapeDataString(value));
    }

    private void EnsureNotDisposed()
    {
      if (_disposed)
      {
        throw new ObjectDisposedException(nameof(XenforoResourceManagerApiClient));
      }
    }

    private static Uri EnsureTrailingSlash(Uri uri)
    {
      if (uri == null)
      {
        throw new ArgumentNullException(nameof(uri));
      }

      if (!uri.IsAbsoluteUri)
      {
        throw new ArgumentException("The API base address must be absolute.", nameof(uri));
      }

      var value = uri.ToString();

      return value.EndsWith("/", StringComparison.Ordinal)
        ? uri
        : new Uri(value + "/");
    }
  }
}
