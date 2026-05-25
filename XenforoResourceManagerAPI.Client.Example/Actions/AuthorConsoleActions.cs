using System.Threading.Tasks;
using XenforoResourceManagerAPI.Client;

namespace XenforoResourceManagerAPI.Client.Example
{
  /// <summary>
  /// Contains console actions for author endpoints.
  /// </summary>
  internal sealed class AuthorConsoleActions : ConsoleActionBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorConsoleActions"/> class.
    /// </summary>
    /// <param name="client">The API client used to call author endpoints.</param>
    /// <param name="ui">The console UI helper.</param>
    public AuthorConsoleActions(XenforoResourceManagerApiClient client, ConsoleUi ui)
      : base(client, ui)
    {
      // No extra state is needed for author actions.
    }

    /// <summary>
    /// Prompts for an author id and prints author details.
    /// </summary>
    public Task GetAsync()
    {
      // Fetch one author by id for exact lookup scenarios.
      int authorId = Ui.ReadPositiveInt("Author id");
      return PrintAsync(Client.Authors.GetAsync(authorId));
    }

    /// <summary>
    /// Prompts for an exact username and prints author details.
    /// </summary>
    public Task FindAsync()
    {
      // The API expects an exact username for the author lookup.
      string name = Ui.ReadRequired("Author username");
      return PrintAsync(Client.Authors.FindAsync(name));
    }
  }
}
