using System.Threading.Tasks;
using XenforoResourceManagerAPI.Client;

namespace XenforoResourceManagerAPI.Client.Example
{
  /// <summary>
  /// Contains console actions for resource category endpoints.
  /// </summary>
  internal sealed class ResourceCategoryConsoleActions : ConsoleActionBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceCategoryConsoleActions"/> class.
    /// </summary>
    /// <param name="client">The API client used to call resource category endpoints.</param>
    /// <param name="ui">The console UI helper.</param>
    public ResourceCategoryConsoleActions(XenforoResourceManagerApiClient client, ConsoleUi ui)
      : base(client, ui)
    {
      // No extra state is needed for resource category actions.
    }

    /// <summary>
    /// Retrieves and prints all available resource categories.
    /// </summary>
    public Task ListAsync()
    {
      // Categories help users find ids for filtered resource listing.
      return PrintAsync(Client.ResourceCategories.ListAsync());
    }
  }
}
