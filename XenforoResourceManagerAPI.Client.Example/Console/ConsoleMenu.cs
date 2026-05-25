using System;
using System.Collections.Generic;
using System.Linq;

namespace XenforoResourceManagerAPI.Client.Example
{
  /// <summary>
  /// Renders the numbered action menu and reads the selected command.
  /// </summary>
  internal sealed class ConsoleMenu
  {
    private readonly IReadOnlyList<MenuAction> _actions;
    private readonly ConsoleUi _ui;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleMenu"/> class.
    /// </summary>
    /// <param name="ui">The console UI helper used for rendering.</param>
    /// <param name="actions">The actions that can be selected by number.</param>
    public ConsoleMenu(ConsoleUi ui, IReadOnlyList<MenuAction> actions)
    {
      // Validate dependencies once so menu rendering can assume they are available.
      _ui = ui ?? throw new ArgumentNullException(nameof(ui));
      _actions = actions ?? throw new ArgumentNullException(nameof(actions));
    }

    /// <summary>
    /// Displays all menu actions and returns the selected action.
    /// </summary>
    /// <returns>The selected action, or null when the entered value is invalid.</returns>
    public MenuAction? ReadAction()
    {
      // Redraw the menu for every selection so the console returns to a predictable state.
      _ui.ClearScreen();
      _ui.WriteHeader("Actions");

      string currentGroup = string.Empty;

      foreach (MenuAction action in _actions)
      {
        // The exit item is printed separately to keep it fixed at zero.
        if (action.IsExit)
        {
          continue;
        }

        // Write a group heading when the next action belongs to a new API area.
        if (!string.Equals(currentGroup, action.Group, StringComparison.Ordinal))
        {
          currentGroup = action.Group;
          Console.WriteLine();
          _ui.WriteAccent(currentGroup);
        }

        Console.WriteLine($"{action.Number,2}. {action.Title}");
      }

      Console.WriteLine();
      Console.WriteLine(" 0. Exit");
      Console.WriteLine();
      Console.Write("Choose action number: ");

      string? input = Console.ReadLine();

      // Invalid input keeps the app alive and returns to the menu after a short pause.
      if (!int.TryParse(input, out int number))
      {
        _ui.WriteError("Please enter a number.");
        _ui.WaitForMenu();
        return null;
      }

      MenuAction? selectedAction = _actions.FirstOrDefault(action => action.Number == number);

      // Unknown numbers are handled like invalid input instead of throwing.
      if (selectedAction == null)
      {
        _ui.WriteError("Unknown action number.");
        _ui.WaitForMenu();
        return null;
      }

      return selectedAction;
    }
  }
}
