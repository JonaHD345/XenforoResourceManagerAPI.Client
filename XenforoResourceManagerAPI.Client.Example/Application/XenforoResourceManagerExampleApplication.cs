using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XenforoResourceManagerAPI.Client;

namespace XenforoResourceManagerAPI.Client.Example
{
  /// <summary>
  /// Coordinates menu selection and action execution for the example app.
  /// </summary>
  internal sealed class XenforoResourceManagerExampleApplication
  {
    private readonly ConsoleMenu _menu;
    private readonly ConsoleUi _ui;

    /// <summary>
    /// Initializes a new instance of the <see cref="XenforoResourceManagerExampleApplication"/> class.
    /// </summary>
    /// <param name="client">The API client shared by all actions.</param>
    /// <param name="ui">The console UI helper.</param>
    public XenforoResourceManagerExampleApplication(
      XenforoResourceManagerApiClient client,
      ConsoleUi ui)
    {
      // Build the menu once because the set of available API actions is static.
      _ui = ui ?? throw new ArgumentNullException(nameof(ui));
      _menu = new ConsoleMenu(_ui, CreateActions(client, _ui));
    }

    /// <summary>
    /// Runs the menu loop until the user exits.
    /// </summary>
    public async Task RunAsync()
    {
      // Show the banner once before the first menu draw.
      _ui.WriteBanner();

      while (true)
      {
        // Every completed action returns here so another number can be entered.
        MenuAction? action = _menu.ReadAction();

        if (action == null)
        {
          continue;
        }

        if (action.IsExit)
        {
          _ui.WriteSuccess("Bye.");
          return;
        }

        // Render each action in a clean section and keep failures inside the menu loop.
        _ui.ClearScreen();
        _ui.WriteHeader(action.Title);

        try
        {
          await action.ExecuteAsync().ConfigureAwait(false);
        }
        catch (XenforoResourceManagerApiException ex)
        {
          _ui.WriteApiException(ex);
        }
        catch (Exception ex)
        {
          _ui.WriteError(ex.Message);
        }

        _ui.WaitForMenu();
      }
    }

    /// <summary>
    /// Creates all menu actions exposed by the example application.
    /// </summary>
    /// <param name="client">The API client shared by all actions.</param>
    /// <param name="ui">The console UI helper.</param>
    /// <returns>The complete list of selectable menu actions.</returns>
    private static IReadOnlyList<MenuAction> CreateActions(
      XenforoResourceManagerApiClient client,
      ConsoleUi ui)
    {
      // Instantiate one small action group per API area to keep menu wiring readable.
      int nextNumber = 1;
      List<MenuAction> actions = new List<MenuAction>();
      ResourceConsoleActions resourceActions = new ResourceConsoleActions(client, ui);
      ResourceCategoryConsoleActions categoryActions = new ResourceCategoryConsoleActions(client, ui);
      ResourceUpdateConsoleActions updateActions = new ResourceUpdateConsoleActions(client, ui);
      AuthorConsoleActions authorActions = new AuthorConsoleActions(client, ui);

      void Add(string group, string title, Func<Task> executeAsync)
      {
        // Assign numbers in registration order so the displayed menu stays predictable.
        actions.Add(new MenuAction(nextNumber++, group, title, executeAsync));
      }

      // Register every public client capability exposed by this package.
      Add("Resources", "List resources", resourceActions.ListAsync);
      Add("Resources", "Get resource", resourceActions.GetAsync);
      Add("Resources", "Get resources by author", resourceActions.GetByAuthorAsync);

      Add("Resource categories", "List resource categories", categoryActions.ListAsync);

      Add("Resource updates", "Get resource update", updateActions.GetAsync);
      Add("Resource updates", "Get resource updates by resource", updateActions.GetByResourceAsync);

      Add("Authors", "Get author", authorActions.GetAsync);
      Add("Authors", "Find author by username", authorActions.FindAsync);

      actions.Add(MenuAction.Exit);

      // Return an immutable view so the menu can render without mutating registrations.
      return actions;
    }
  }
}
