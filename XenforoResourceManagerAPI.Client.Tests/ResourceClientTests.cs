namespace XenforoResourceManagerAPI.Client.Tests
{
  public sealed class ResourceClientTests : EndpointClientTestBase
  {
    [Fact]
    public async Task ListAsync_WithoutOptionalFilters_SendsExpectedRequestAndReturnsResources()
    {
      // Arrange
      var (client, handler) = CreateClient("[{\"id\":10,\"title\":\"Resource\"}]");
      using (client)
      {
        // Act
        var resources = await client.Resources.ListAsync();

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/index.php?action=listResources", request.RequestUri?.PathAndQuery);
        Assert.Equal("application/json", request.Accept);
        var resource = Assert.Single(resources);
        Assert.Equal(10, resource.Id);
        Assert.Equal("Resource", resource.Title);
      }
    }

    [Fact]
    public async Task ListAsync_WithCategoryAndPage_SendsExpectedRequestAndReturnsResources()
    {
      // Arrange
      var (client, handler) = CreateClient("[{\"id\":11,\"title\":\"Filtered Resource\"}]");
      using (client)
      {
        // Act
        var resources = await client.Resources.ListAsync(category: 4, page: 2);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/index.php?action=listResources&category=4&page=2", request.RequestUri?.PathAndQuery);
        Assert.Equal("application/json", request.Accept);
        var resource = Assert.Single(resources);
        Assert.Equal(11, resource.Id);
        Assert.Equal("Filtered Resource", resource.Title);
      }
    }

    [Theory]
    [InlineData(0, null, "category")]
    [InlineData(-1, null, "category")]
    [InlineData(null, 0, "page")]
    [InlineData(null, -1, "page")]
    public async Task ListAsync_WithInvalidOptionalFilter_ThrowsArgumentOutOfRangeException(int? category, int? page, string parameterName)
    {
      // Arrange
      var (client, handler) = CreateClient("[]");
      using (client)
      {
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Resources.ListAsync(category, page));

        // Assert
        Assert.Equal(parameterName, exception.ParamName);
        Assert.Empty(handler.Requests);
      }
    }

    [Fact]
    public async Task GetAsync_WithResourceId_SendsExpectedRequestAndReturnsResource()
    {
      // Arrange
      var (client, handler) = CreateClient("{\"id\":20,\"title\":\"Single Resource\"}");
      using (client)
      {
        // Act
        var resource = await client.Resources.GetAsync(20);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/index.php?action=getResource&id=20", request.RequestUri?.PathAndQuery);
        Assert.Equal("application/json", request.Accept);
        Assert.Equal(20, resource.Id);
        Assert.Equal("Single Resource", resource.Title);
      }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetAsync_WithNonPositiveResourceId_ThrowsArgumentOutOfRangeException(int resourceId)
    {
      // Arrange
      var (client, handler) = CreateClient("{}");
      using (client)
      {
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Resources.GetAsync(resourceId));

        // Assert
        Assert.Equal("resourceId", exception.ParamName);
        Assert.Empty(handler.Requests);
      }
    }

    [Fact]
    public async Task GetByAuthorAsync_WithoutPage_SendsExpectedRequestAndReturnsResources()
    {
      // Arrange
      var (client, handler) = CreateClient("[{\"id\":30,\"title\":\"Author Resource\"}]");
      using (client)
      {
        // Act
        var resources = await client.Resources.GetByAuthorAsync(8);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/index.php?action=getResourcesByAuthor&id=8", request.RequestUri?.PathAndQuery);
        Assert.Equal("application/json", request.Accept);
        var resource = Assert.Single(resources);
        Assert.Equal(30, resource.Id);
        Assert.Equal("Author Resource", resource.Title);
      }
    }

    [Fact]
    public async Task GetByAuthorAsync_WithPage_SendsExpectedRequestAndReturnsResources()
    {
      // Arrange
      var (client, handler) = CreateClient("[{\"id\":31,\"title\":\"Paged Author Resource\"}]");
      using (client)
      {
        // Act
        var resources = await client.Resources.GetByAuthorAsync(8, page: 3);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/index.php?action=getResourcesByAuthor&id=8&page=3", request.RequestUri?.PathAndQuery);
        Assert.Equal("application/json", request.Accept);
        var resource = Assert.Single(resources);
        Assert.Equal(31, resource.Id);
        Assert.Equal("Paged Author Resource", resource.Title);
      }
    }

    [Theory]
    [InlineData(0, null, "authorId")]
    [InlineData(-1, null, "authorId")]
    [InlineData(8, 0, "page")]
    [InlineData(8, -1, "page")]
    public async Task GetByAuthorAsync_WithInvalidArgument_ThrowsArgumentOutOfRangeException(int authorId, int? page, string parameterName)
    {
      // Arrange
      var (client, handler) = CreateClient("[]");
      using (client)
      {
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Resources.GetByAuthorAsync(authorId, page));

        // Assert
        Assert.Equal(parameterName, exception.ParamName);
        Assert.Empty(handler.Requests);
      }
    }
  }
}
