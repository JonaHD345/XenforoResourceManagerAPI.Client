using System;
using System.Threading.Tasks;

namespace XenforoResourceManagerAPI.Client.Example
{
  /// <summary>
  /// Describes one selectable command in the example menu.
  /// </summary>
  internal sealed class MenuAction
  {
    /// <summary>
    /// Gets the menu item that exits the example application.
    /// </summary>
    public static MenuAction Exit { get; } = new MenuAction(0, "General", "Exit", () => Task.CompletedTask, true);

    /// <summary>
    /// Initializes a new instance of the <see cref="MenuAction"/> class.
    /// </summary>
    /// <param name="number">The number entered by the user to select the action.</param>
    /// <param name="group">The group heading shown in the menu.</param>
    /// <param name="title">The action title shown in the menu.</param>
    /// <param name="executeAsync">The delegate that executes the action.</param>
    /// <param name="isExit">A value indicating whether this action exits the application.</param>
    public MenuAction(int number, string group, string title, Func<Task> executeAsync, bool isExit = false)
    {
      // Copy all display and execution values into immutable properties.
      Number = number;
      Group = group;
      Title = title;
      ExecuteAsync = executeAsync;
      IsExit = isExit;
    }

    /// <summary>
    /// Gets the number entered by the user to select the action.
    /// </summary>
    public int Number { get; }

    /// <summary>
    /// Gets the group heading shown in the menu.
    /// </summary>
    public string Group { get; }

    /// <summary>
    /// Gets the action title shown in the menu.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the delegate that executes the action.
    /// </summary>
    public Func<Task> ExecuteAsync { get; }

    /// <summary>
    /// Gets a value indicating whether this action exits the application.
    /// </summary>
    public bool IsExit { get; }
  }
}
