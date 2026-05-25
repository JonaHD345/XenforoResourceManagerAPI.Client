using System.Threading.Tasks;
using XenforoResourceManagerAPI.Client;

namespace XenforoResourceManagerAPI.Client.Example
{
  /// <summary>
  /// Contains console actions for resource update endpoints.
  /// </summary>
  internal sealed class ResourceUpdateConsoleActions : ConsoleActionBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceUpdateConsoleActions"/> class.
    /// </summary>
    /// <param name="client">The API client used to call resource update endpoints.</param>
    /// <param name="ui">The console UI helper.</param>
    public ResourceUpdateConsoleActions(XenforoResourceManagerApiClient client, ConsoleUi ui)
      : base(client, ui)
    {
      // No extra state is needed for resource update actions.
    }

    /// <summary>
    /// Prompts for a resource update id and prints update details.
    /// </summary>
    public Task GetAsync()
    {
      // Fetch one update by id so users can inspect the full update payload.
      int updateId = Ui.ReadPositiveInt("Resource update id");
      return PrintAsync(Client.ResourceUpdates.GetAsync(updateId));
    }

    /// <summary>
    /// Prompts for a resource id and prints its updates.
    /// </summary>
    public Task GetByResourceAsync()
    {
      // Page is optional because the API starts with its default first page.
      int resourceId = Ui.ReadPositiveInt("Resource id");
      int? page = Ui.ReadOptionalPositiveInt("Page (optional)");

      return PrintAsync(Client.ResourceUpdates.GetByResourceAsync(resourceId, page));
    }
  }
}
