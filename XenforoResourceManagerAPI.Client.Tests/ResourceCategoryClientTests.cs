namespace XenforoResourceManagerAPI.Client.Tests
{
  public sealed class ResourceCategoryClientTests : EndpointClientTestBase
  {
    [Fact]
    public async Task ListAsync_SendsExpectedRequestAndReturnsCategories()
    {
      // Arrange
      var (client, handler) = CreateClient("[{\"id\":4,\"title\":\"Tools\",\"description\":\"Tooling\"}]");
      using (client)
      {
        // Act
        var categories = await client.ResourceCategories.ListAsync();

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/index.php?action=listResourceCategories", request.RequestUri?.PathAndQuery);
        Assert.Equal("application/json", request.Accept);
        var category = Assert.Single(categories);
        Assert.Equal(4, category.Id);
        Assert.Equal("Tools", category.Title);
        Assert.Equal("Tooling", category.Description);
      }
    }
  }
}
