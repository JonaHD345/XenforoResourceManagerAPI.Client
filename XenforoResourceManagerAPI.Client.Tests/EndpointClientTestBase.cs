namespace XenforoResourceManagerAPI.Client.Tests
{
  public abstract class EndpointClientTestBase
  {
    protected static (XenforoResourceManagerApiClient Client, TestHttpMessageHandler Handler) CreateClient(string jsonResponse)
    {
      var handler = new TestHttpMessageHandler((_, _) => TestHttpMessageHandler.CreateJsonResponse(jsonResponse));
      var httpClient = new HttpClient(handler);
      var options = new XenforoResourceManagerApiClientOptions
      {
        BaseAddress = new Uri("https://example.test/api")
      };

      return (new XenforoResourceManagerApiClient(httpClient, options), handler);
    }
  }
}
