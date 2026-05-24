namespace XenforoResourceManagerAPI.Client.Tests
{
  public sealed class XenforoResourceManagerApiClientTests
  {
    [Fact]
    public void Constructor_Default_InitializesEndpointClients()
    {
      // Arrange

      // Act
      using var client = new XenforoResourceManagerApiClient();

      // Assert
      Assert.NotNull(client.Resources);
      Assert.NotNull(client.ResourceCategories);
      Assert.NotNull(client.ResourceUpdates);
      Assert.NotNull(client.Authors);
    }

    [Fact]
    public void Constructor_WithOptions_InitializesEndpointClients()
    {
      // Arrange
      var options = new XenforoResourceManagerApiClientOptions
      {
        BaseAddress = new Uri("https://example.test/api")
      };

      // Act
      using var client = new XenforoResourceManagerApiClient(options);

      // Assert
      Assert.NotNull(client.Resources);
      Assert.NotNull(client.ResourceCategories);
      Assert.NotNull(client.ResourceUpdates);
      Assert.NotNull(client.Authors);
    }

    [Fact]
    public async Task Constructor_WithHttpClientAndOptions_UsesProvidedOptions()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("[]"));
      var httpClient = new HttpClient(handler);
      var options = new XenforoResourceManagerApiClientOptions
      {
        BaseAddress = new Uri("https://example.test/api")
      };
      using var client = new XenforoResourceManagerApiClient(httpClient, options);

      // Act
      await client.ResourceCategories.ListAsync();

      // Assert
      var request = Assert.Single(handler.Requests);
      Assert.Equal("/api/index.php?action=listResourceCategories", request.RequestUri?.PathAndQuery);
    }

    [Fact]
    public async Task Constructor_WithHttpClientAndNullOptions_UsesDefaultOptions()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("[]"));
      var httpClient = new HttpClient(handler);
      using var client = new XenforoResourceManagerApiClient(httpClient, null);

      // Act
      await client.ResourceCategories.ListAsync();

      // Assert
      var request = Assert.Single(handler.Requests);
      Assert.Equal("https://api.spigotmc.org/simple/0.2/index.php?action=listResourceCategories", request.RequestUri?.ToString());
    }

    [Fact]
    public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
    {
      // Arrange
      HttpClient? httpClient = null;

      // Act
      var exception = Assert.Throws<ArgumentNullException>(() => new XenforoResourceManagerApiClient(httpClient!));

      // Assert
      Assert.Equal("httpClient", exception.ParamName);
    }

    [Fact]
    public async Task Dispose_AfterClientDisposed_PreventsApiRequests()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("{}"));
      using var httpClient = new HttpClient(handler);
      var client = new XenforoResourceManagerApiClient(httpClient);

      // Act
      client.Dispose();
      var exception = await Assert.ThrowsAsync<ObjectDisposedException>(() => client.Resources.GetAsync(1));

      // Assert
      Assert.Equal(nameof(XenforoResourceManagerApiClient), exception.ObjectName);
    }

    [Fact]
    public async Task Dispose_WithProvidedHttpClient_DoesNotDisposeHttpClient()
    {
      // Arrange
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse("{}"));
      using var httpClient = new HttpClient(handler);
      var client = new XenforoResourceManagerApiClient(httpClient);

      // Act
      client.Dispose();
      using var response = await httpClient.GetAsync("https://example.test/manual");

      // Assert
      Assert.True(response.IsSuccessStatusCode);
      Assert.False(handler.IsDisposed);
    }
  }
}
