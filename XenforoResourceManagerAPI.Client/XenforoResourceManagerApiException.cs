using System;
using System.Net;

namespace XenforoResourceManagerAPI.Client
{
  /// <summary>
  /// Represents an error returned by the XenForo Resource Manager API or produced while reading an API response.
  /// </summary>
  public class XenforoResourceManagerApiException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="XenforoResourceManagerApiException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public XenforoResourceManagerApiException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XenforoResourceManagerApiException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="innerException">The exception that caused this exception.</param>
    public XenforoResourceManagerApiException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XenforoResourceManagerApiException"/> class.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="statusCode">The HTTP status code returned by the API.</param>
    /// <param name="responseContent">The raw response content returned by the API.</param>
    public XenforoResourceManagerApiException(string message, HttpStatusCode statusCode, string? responseContent)
      : base(message)
    {
      StatusCode = statusCode;
      ResponseContent = responseContent;
    }

    /// <summary>
    /// Gets the HTTP status code returned by the API.
    /// </summary>
    public HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// Gets the raw response content returned by the API.
    /// </summary>
    public string? ResponseContent { get; }
  }
}
