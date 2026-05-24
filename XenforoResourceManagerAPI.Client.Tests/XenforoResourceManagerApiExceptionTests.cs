using System.Net;

namespace XenforoResourceManagerAPI.Client.Tests
{
  public sealed class XenforoResourceManagerApiExceptionTests
  {
    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
      // Arrange
      const string message = "API error.";

      // Act
      var exception = new XenforoResourceManagerApiException(message);

      // Assert
      Assert.Equal(message, exception.Message);
      Assert.Null(exception.InnerException);
      Assert.Null(exception.StatusCode);
      Assert.Null(exception.ResponseContent);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsInnerException()
    {
      // Arrange
      var innerException = new InvalidOperationException("Inner error.");

      // Act
      var exception = new XenforoResourceManagerApiException("API error.", innerException);

      // Assert
      Assert.Equal(innerException, exception.InnerException);
      Assert.Null(exception.StatusCode);
      Assert.Null(exception.ResponseContent);
    }

    [Fact]
    public void Constructor_WithHttpStatusAndResponseContent_SetsApiErrorDetails()
    {
      // Arrange
      const string responseContent = "{\"error\":\"missing\"}";

      // Act
      var exception = new XenforoResourceManagerApiException("API error.", HttpStatusCode.NotFound, responseContent);

      // Assert
      Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
      Assert.Equal(responseContent, exception.ResponseContent);
    }
  }
}
