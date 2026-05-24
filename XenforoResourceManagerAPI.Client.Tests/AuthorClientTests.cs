namespace XenforoResourceManagerAPI.Client.Tests
{
  public sealed class AuthorClientTests : EndpointClientTestBase
  {
    [Fact]
    public async Task GetAsync_WithAuthorId_SendsExpectedRequestAndReturnsAuthor()
    {
      // Arrange
      var (client, handler) = CreateClient("{\"id\":42,\"username\":\"Author\",\"resource_count\":3}");
      using (client)
      {
        // Act
        var author = await client.Authors.GetAsync(42);

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/index.php?action=getAuthor&id=42", request.RequestUri?.PathAndQuery);
        Assert.Equal("application/json", request.Accept);
        Assert.Equal(42, author.Id);
        Assert.Equal("Author", author.Username);
        Assert.Equal(3, author.ResourceCount);
      }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task GetAsync_WithNonPositiveAuthorId_ThrowsArgumentOutOfRangeException(int authorId)
    {
      // Arrange
      var (client, handler) = CreateClient("{}");
      using (client)
      {
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => client.Authors.GetAsync(authorId));

        // Assert
        Assert.Equal("authorId", exception.ParamName);
        Assert.Empty(handler.Requests);
      }
    }

    [Fact]
    public async Task FindAsync_WithName_SendsExpectedRequestAndReturnsAuthor()
    {
      // Arrange
      var (client, handler) = CreateClient("{\"id\":7,\"username\":\"Exact Name\"}");
      using (client)
      {
        // Act
        var author = await client.Authors.FindAsync("Exact Name");

        // Assert
        var request = Assert.Single(handler.Requests);
        Assert.Equal(HttpMethod.Get, request.Method);
        Assert.Equal("/api/index.php?action=findAuthor&name=Exact%20Name", request.RequestUri?.PathAndQuery);
        Assert.Equal("application/json", request.Accept);
        Assert.Equal(7, author.Id);
        Assert.Equal("Exact Name", author.Username);
      }
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task FindAsync_WithMissingName_ThrowsArgumentException(string? name)
    {
      // Arrange
      var (client, handler) = CreateClient("{}");
      using (client)
      {
        // Act
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => client.Authors.FindAsync(name!));

        // Assert
        Assert.Equal("name", exception.ParamName);
        Assert.Empty(handler.Requests);
      }
    }
  }
}
