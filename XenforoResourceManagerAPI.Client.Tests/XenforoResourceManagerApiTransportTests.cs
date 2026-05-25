using System.Globalization;
using System.Net;

namespace XenforoResourceManagerAPI.Client.Tests
{
  public sealed class XenforoResourceManagerApiTransportTests
  {
    [Fact]
    public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
    {
      // Arrange
      var options = new XenforoResourceManagerApiClientOptions();

      // Act
      var exception = Assert.Throws<ArgumentNullException>(() => new XenforoResourceManagerApiTransport(null!, options, false));

      // Assert
      Assert.Equal("httpClient", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullOptions_ThrowsArgumentNullException()
    {
      // Arrange
      using var httpClient = new HttpClient();

      // Act
      var exception = Assert.Throws<ArgumentNullException>(() => new XenforoResourceManagerApiTransport(httpClient, null!, false));

      // Assert
      Assert.Equal("options", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithRelativeBaseAddress_ThrowsArgumentException()
    {
      // Arrange
      using var httpClient = new HttpClient();
      var options = new XenforoResourceManagerApiClientOptions
      {
        BaseAddress = new Uri("relative/path", UriKind.Relative)
      };

      // Act
      var exception = Assert.Throws<ArgumentException>(() => new XenforoResourceManagerApiTransport(httpClient, options, false));

      // Assert
      Assert.Equal("uri", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithoutHttpClientBaseAddress_AppliesOptionsBaseAddressWithTrailingSlash()
    {
      // Arrange
      using var httpClient = new HttpClient();
      var options = new XenforoResourceManagerApiClientOptions
      {
        BaseAddress = new Uri("https://example.test/api")
      };

      // Act
      using var transport = new XenforoResourceManagerApiTransport(httpClient, options, false);

      // Assert
      Assert.Equal(new Uri("https://example.test/api/"), httpClient.BaseAddress);
    }

    [Fact]
    public void Constructor_WithExistingHttpClientBaseAddress_KeepsExistingBaseAddress()
    {
      // Arrange
      using var httpClient = new HttpClient
      {
        BaseAddress = new Uri("https://existing.test/root/")
      };
      var options = new XenforoResourceManagerApiClientOptions
      {
        BaseAddress = new Uri("https://example.test/api")
      };

      // Act
      using var transport = new XenforoResourceManagerApiTransport(httpClient, options, false);

      // Assert
      Assert.Equal(new Uri("https://existing.test/root/"), httpClient.BaseAddress);
    }

    [Fact]
    public void Dispose_WithOwnedHttpClient_DisposesHttpClient()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("{}"));
      var httpClient = new HttpClient(handler);
      var transport = new XenforoResourceManagerApiTransport(httpClient, new XenforoResourceManagerApiClientOptions(), true);

      // Act
      transport.Dispose();
      transport.Dispose();

      // Assert
      Assert.True(handler.IsDisposed);
    }

    [Fact]
    public void Dispose_WithExternalHttpClient_DoesNotDisposeHttpClient()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("{}"));
      using var httpClient = new HttpClient(handler);
      var transport = new XenforoResourceManagerApiTransport(httpClient, new XenforoResourceManagerApiClientOptions(), false);

      // Act
      transport.Dispose();

      // Assert
      Assert.False(handler.IsDisposed);
    }

    [Fact]
    public async Task SendAsync_WithSuccessfulResponse_SendsRequestAndReturnsDeserializedResponse()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("{\"id\":5,\"title\":\"Transport Resource\"}"));
      using var httpClient = new HttpClient(handler);
      using var transport = new XenforoResourceManagerApiTransport(
        httpClient,
        new XenforoResourceManagerApiClientOptions { BaseAddress = new Uri("https://example.test/api") },
        false);
      using var cancellationTokenSource = new CancellationTokenSource();

      // Act
      var resource = await transport.SendAsync<Resource>(
        HttpMethod.Get,
        "getResource",
        cancellationTokenSource.Token,
        XenforoResourceManagerApiTransport.CreateParameter("id", "5"));

      // Assert
      var request = Assert.Single(handler.Requests);
      Assert.Equal(HttpMethod.Get, request.Method);
      Assert.Equal("/api/index.php?action=getResource&id=5", request.RequestUri?.PathAndQuery);
      Assert.Equal("application/json", request.Accept);
      Assert.Equal("XenforoResourceManagerAPI.Client/1.0.1 (+https://github.com/JonaHD345/XenforoResourceManagerAPI.NET)", request.UserAgent);
      Assert.True(Assert.Single(handler.CancellationTokens).CanBeCanceled);
      Assert.Equal(5, resource.Id);
      Assert.Equal("Transport Resource", resource.Title);
    }

    [Fact]
    public async Task SendAsync_WithHttpClientUserAgent_KeepsCustomUserAgent()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("{\"id\":5,\"title\":\"Transport Resource\"}"));
      using var httpClient = new HttpClient(handler);
      httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("CustomClient/2.0");
      using var transport = new XenforoResourceManagerApiTransport(
        httpClient,
        new XenforoResourceManagerApiClientOptions { BaseAddress = new Uri("https://example.test/api") },
        false);

      // Act
      await transport.SendAsync<Resource>(
        HttpMethod.Get,
        "getResource",
        CancellationToken.None,
        XenforoResourceManagerApiTransport.CreateParameter("id", "5"));

      // Assert
      var request = Assert.Single(handler.Requests);
      Assert.Equal("CustomClient/2.0", request.UserAgent);
    }

    [Fact]
    public async Task SendAsync_WithUrlSensitiveParameters_EscapesQueryValues()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("{\"id\":1,\"username\":\"A&B\"}"));
      using var httpClient = new HttpClient(handler);
      using var transport = new XenforoResourceManagerApiTransport(
        httpClient,
        new XenforoResourceManagerApiClientOptions { BaseAddress = new Uri("https://example.test/api") },
        false);

      // Act
      await transport.SendAsync<Author>(
        HttpMethod.Get,
        "findAuthor",
        CancellationToken.None,
        XenforoResourceManagerApiTransport.CreateParameter("name", "A&B Name"));

      // Assert
      var request = Assert.Single(handler.Requests);
      Assert.Equal("/api/index.php?action=findAuthor&name=A%26B%20Name", request.RequestUri?.PathAndQuery);
    }

    [Fact]
    public async Task SendAsync_WithNullParameter_SkipsParameter()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("[]"));
      using var httpClient = new HttpClient(handler);
      using var transport = new XenforoResourceManagerApiTransport(
        httpClient,
        new XenforoResourceManagerApiClientOptions { BaseAddress = new Uri("https://example.test/api") },
        false);

      // Act
      await transport.SendAsync<List<Resource>>(
        HttpMethod.Get,
        "listResources",
        CancellationToken.None,
        XenforoResourceManagerApiTransport.CreateParameter("category", null));

      // Assert
      var request = Assert.Single(handler.Requests);
      Assert.Equal("/api/index.php?action=listResources", request.RequestUri?.PathAndQuery);
    }

    [Fact]
    public async Task SendAsync_WithHttpError_ThrowsApiExceptionWithStatusAndContent()
    {
      // Arrange
      const string responseContent = "{\"error\":\"missing\"}";
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateTextResponse(responseContent, HttpStatusCode.NotFound));
      using var httpClient = new HttpClient(handler);
      using var transport = new XenforoResourceManagerApiTransport(httpClient, new XenforoResourceManagerApiClientOptions(), false);

      // Act
      var exception = await Assert.ThrowsAsync<XenforoResourceManagerApiException>(() => transport.SendAsync<Resource>(HttpMethod.Get, "getResource", CancellationToken.None));

      // Assert
      Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
      Assert.Equal(responseContent, exception.ResponseContent);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SendAsync_WithEmptyResponse_ThrowsApiException(string responseContent)
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateTextResponse(responseContent));
      using var httpClient = new HttpClient(handler);
      using var transport = new XenforoResourceManagerApiTransport(httpClient, new XenforoResourceManagerApiClientOptions(), false);

      // Act
      var exception = await Assert.ThrowsAsync<XenforoResourceManagerApiException>(() => transport.SendAsync<Resource>(HttpMethod.Get, "getResource", CancellationToken.None));

      // Assert
      Assert.Equal("The XenForo Resource Manager API returned an empty response.", exception.Message);
    }

    [Fact]
    public async Task SendAsync_WithInvalidJson_ThrowsApiExceptionWithInnerException()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateTextResponse("{"));
      using var httpClient = new HttpClient(handler);
      using var transport = new XenforoResourceManagerApiTransport(httpClient, new XenforoResourceManagerApiClientOptions(), false);

      // Act
      var exception = await Assert.ThrowsAsync<XenforoResourceManagerApiException>(() => transport.SendAsync<Resource>(HttpMethod.Get, "getResource", CancellationToken.None));

      // Assert
      Assert.NotNull(exception.InnerException);
      Assert.Equal("The XenForo Resource Manager API response could not be deserialized.", exception.Message);
    }

    [Fact]
    public async Task SendAsync_WithJsonNull_ThrowsApiException()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("null"));
      using var httpClient = new HttpClient(handler);
      using var transport = new XenforoResourceManagerApiTransport(httpClient, new XenforoResourceManagerApiClientOptions(), false);

      // Act
      var exception = await Assert.ThrowsAsync<XenforoResourceManagerApiException>(() => transport.SendAsync<Resource>(HttpMethod.Get, "getResource", CancellationToken.None));

      // Assert
      Assert.Equal("The XenForo Resource Manager API response could not be deserialized.", exception.Message);
    }

    [Fact]
    public async Task SendAsync_AfterDispose_ThrowsObjectDisposedException()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("{}"));
      using var httpClient = new HttpClient(handler);
      var transport = new XenforoResourceManagerApiTransport(httpClient, new XenforoResourceManagerApiClientOptions(), false);

      // Act
      transport.Dispose();
      var exception = await Assert.ThrowsAsync<ObjectDisposedException>(() => transport.SendAsync<Resource>(HttpMethod.Get, "getResource", CancellationToken.None));

      // Assert
      Assert.Equal(nameof(XenforoResourceManagerApiClient), exception.ObjectName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task SendAsync_WithMissingAction_ThrowsArgumentException(string? action)
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("{}"));
      using var httpClient = new HttpClient(handler);
      using var transport = new XenforoResourceManagerApiTransport(httpClient, new XenforoResourceManagerApiClientOptions(), false);

      // Act
      var exception = await Assert.ThrowsAsync<ArgumentException>(() => transport.SendAsync<Resource>(HttpMethod.Get, action!, CancellationToken.None));

      // Assert
      Assert.Equal("action", exception.ParamName);
      Assert.Empty(handler.Requests);
    }

    [Fact]
    public void CreateParameter_WithNameAndValue_ReturnsKeyValuePair()
    {
      // Arrange
      const string name = "page";
      const string value = "2";

      // Act
      var parameter = XenforoResourceManagerApiTransport.CreateParameter(name, value);

      // Assert
      Assert.Equal(name, parameter.Key);
      Assert.Equal(value, parameter.Value);
    }

    [Fact]
    public void ValidatePositiveNumber_WithPositiveValue_DoesNotThrow()
    {
      // Arrange
      const int value = 1;

      // Act
      var exception = Record.Exception(() => XenforoResourceManagerApiTransport.ValidatePositiveNumber(value, "value"));

      // Assert
      Assert.Null(exception);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ValidatePositiveNumber_WithNonPositiveValue_ThrowsArgumentOutOfRangeException(int value)
    {
      // Arrange

      // Act
      var exception = Assert.Throws<ArgumentOutOfRangeException>(() => XenforoResourceManagerApiTransport.ValidatePositiveNumber(value, "value"));

      // Assert
      Assert.Equal("value", exception.ParamName);
      Assert.Equal(value, exception.ActualValue);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(1)]
    public void ValidateOptionalPositiveNumber_WithNullOrPositiveValue_DoesNotThrow(int? value)
    {
      // Arrange

      // Act
      var exception = Record.Exception(() => XenforoResourceManagerApiTransport.ValidateOptionalPositiveNumber(value, "value"));

      // Assert
      Assert.Null(exception);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ValidateOptionalPositiveNumber_WithNonPositiveValue_ThrowsArgumentOutOfRangeException(int value)
    {
      // Arrange

      // Act
      var exception = Assert.Throws<ArgumentOutOfRangeException>(() => XenforoResourceManagerApiTransport.ValidateOptionalPositiveNumber(value, "value"));

      // Assert
      Assert.Equal("value", exception.ParamName);
      Assert.Equal(value, exception.ActualValue);
    }

    [Fact]
    public void ValidateRequiredString_WithValue_DoesNotThrow()
    {
      // Arrange
      const string value = "resource";

      // Act
      var exception = Record.Exception(() => XenforoResourceManagerApiTransport.ValidateRequiredString(value, "value"));

      // Assert
      Assert.Null(exception);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ValidateRequiredString_WithMissingValue_ThrowsArgumentException(string? value)
    {
      // Arrange

      // Act
      var exception = Assert.Throws<ArgumentException>(() => XenforoResourceManagerApiTransport.ValidateRequiredString(value, "value"));

      // Assert
      Assert.Equal("value", exception.ParamName);
    }

    [Fact]
    public void FormatNumber_WithCurrentCulture_ReturnsInvariantNumber()
    {
      // Arrange
      var originalCulture = CultureInfo.CurrentCulture;
      CultureInfo.CurrentCulture = new CultureInfo("de-DE");

      try
      {
        // Act
        var value = XenforoResourceManagerApiTransport.FormatNumber(123);

        // Assert
        Assert.Equal("123", value);
      }
      finally
      {
        CultureInfo.CurrentCulture = originalCulture;
      }
    }

    [Fact]
    public void FormatOptionalNumber_WithValue_ReturnsInvariantNumber()
    {
      // Arrange
      const int number = 456;

      // Act
      var value = XenforoResourceManagerApiTransport.FormatOptionalNumber(number);

      // Assert
      Assert.Equal("456", value);
    }

    [Fact]
    public void FormatOptionalNumber_WithNull_ReturnsNull()
    {
      // Arrange
      int? number = null;

      // Act
      var value = XenforoResourceManagerApiTransport.FormatOptionalNumber(number);

      // Assert
      Assert.Null(value);
    }
  }
}
