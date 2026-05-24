namespace XenforoResourceManagerAPI.Client.Tests
{
  public sealed class ResourceUpdateClientTests : EndpointClientTestBase
  {
    [Fact]
    public async Task GetAsync_WithResourceUpdateId_SendsExpectedRequestAndReturnsUpdate()
    {
      // Arrange
      var (client, handler) = CreateClient("{\"id\":50,\"resource_id\":20,\"title\":\"Update\"}");
      using (client)
      {
        // Act
        var update = await client.ResourceUpdates.GetAsync(50);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/index.php?action=getResourceUpdate&id=50", request.RequestUri?.PathAndQuery);
        Assert.Equal("application/json", request.Accept);
        Assert.Equal(50, update.Id);
        Assert.Equal(20, update.ResourceId);
        Assert.Equal("Update", update.Title);
      }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetAsync_WithNonPositiveResourceUpdateId_ThrowsArgumentOutOfRangeException(int resourceUpdateId)
    {
      // Arrange
      var (client, handler) = CreateClient("{}");
      using (client)
      {
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.ResourceUpdates.GetAsync(resourceUpdateId));

        // Assert
        Assert.Equal("resourceUpdateId", exception.ParamName);
        Assert.Empty(handler.Requests);
      }
    }

    [Fact]
    public async Task GetByResourceAsync_WithoutPage_SendsExpectedRequestAndReturnsUpdates()
    {
      // Arrange
      var (client, handler) = CreateClient("[{\"id\":51,\"resource_id\":21,\"title\":\"Resource Update\"}]");
      using (client)
      {
        // Act
        var updates = await client.ResourceUpdates.GetByResourceAsync(21);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/index.php?action=getResourceUpdates&id=21", request.RequestUri?.PathAndQuery);
        Assert.Equal("application/json", request.Accept);
        var update = Assert.Single(updates);
        Assert.Equal(51, update.Id);
        Assert.Equal(21, update.ResourceId);
        Assert.Equal("Resource Update", update.Title);
      }
    }

    [Fact]
    public async Task GetByResourceAsync_WithPage_SendsExpectedRequestAndReturnsUpdates()
    {
      // Arrange
      var (client, handler) = CreateClient("[{\"id\":52,\"resource_id\":21,\"title\":\"Paged Resource Update\"}]");
      using (client)
      {
        // Act
        var updates = await client.ResourceUpdates.GetByResourceAsync(21, page: 4);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/index.php?action=getResourceUpdates&id=21&page=4", request.RequestUri?.PathAndQuery);
        Assert.Equal("application/json", request.Accept);
        var update = Assert.Single(updates);
        Assert.Equal(52, update.Id);
        Assert.Equal(21, update.ResourceId);
        Assert.Equal("Paged Resource Update", update.Title);
      }
    }

    [Theory]
    [InlineData(0, null, "resourceId")]
    [InlineData(-1, null, "resourceId")]
    [InlineData(21, 0, "page")]
    [InlineData(21, -1, "page")]
    public async Task GetByResourceAsync_WithInvalidArgument_ThrowsArgumentOutOfRangeException(int resourceId, int? page, string parameterName)
    {
      // Arrange
      var (client, handler) = CreateClient("[]");
      using (client)
      {
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.ResourceUpdates.GetByResourceAsync(resourceId, page));

        // Assert
        Assert.Equal(parameterName, exception.ParamName);
        Assert.Empty(handler.Requests);
      }
    }
  }
}
