using System.Threading.Tasks;
using XenforoResourceManagerAPI.Client;

namespace XenforoResourceManagerAPI.Client.Example
{
  /// <summary>
  /// Contains console actions for resource endpoints.
  /// </summary>
  internal sealed class ResourceConsoleActions : ConsoleActionBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceConsoleActions"/> class.
    /// </summary>
    /// <param name="client">The API client used to call resource endpoints.</param>
    /// <param name="ui">The console UI helper.</param>
    public ResourceConsoleActions(XenforoResourceManagerApiClient client, ConsoleUi ui)
      : base(client, ui)
    {
      // No extra state is needed for resource actions.
    }

    /// <summary>
    /// Retrieves and prints resources with optional category and page filters.
    /// </summary>
    public Task ListAsync()
    {
      // Optional filters map directly to the API query parameters.
      int? categoryId = Ui.ReadOptionalPositiveInt("Category id (optional)");
      int? page = Ui.ReadOptionalPositiveInt("Page (optional)");

      return PrintAsync(Client.Resources.ListAsync(categoryId, page));
    }

    /// <summary>
    /// Prompts for a resource id and prints resource details.
    /// </summary>
    public Task GetAsync()
    {
      // Fetch one resource by id so users can inspect all returned fields.
      int resourceId = Ui.ReadPositiveInt("Resource id");
      return PrintAsync(Client.Resources.GetAsync(resourceId));
    }

    /// <summary>
    /// Prompts for an author id and prints that author's resources.
    /// </summary>
    public Task GetByAuthorAsync()
    {
      // Page is optional because the API starts with its default first page.
      int authorId = Ui.ReadPositiveInt("Author id");
      int? page = Ui.ReadOptionalPositiveInt("Page (optional)");

      return PrintAsync(Client.Resources.GetByAuthorAsync(authorId, page));
    }
  }
}
