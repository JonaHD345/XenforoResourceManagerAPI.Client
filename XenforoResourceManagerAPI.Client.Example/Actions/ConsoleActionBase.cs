using System;
using System.Threading.Tasks;
using XenforoResourceManagerAPI.Client;

namespace XenforoResourceManagerAPI.Client.Example
{
  /// <summary>
  /// Provides shared client and output helpers for console action classes.
  /// </summary>
  internal abstract class ConsoleActionBase
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleActionBase"/> class.
    /// </summary>
    /// <param name="client">The API client used by the action class.</param>
    /// <param name="ui">The console UI helper used for prompts and output.</param>
    protected ConsoleActionBase(XenforoResourceManagerApiClient client, ConsoleUi ui)
    {
      // Store validated dependencies so derived actions can stay focused on API calls.
      Client = client ?? throw new ArgumentNullException(nameof(client));
      Ui = ui ?? throw new ArgumentNullException(nameof(ui));
    }

    protected XenforoResourceManagerApiClient Client { get; }

    protected ConsoleUi Ui { get; }

    /// <summary>
    /// Executes an API operation and writes its result as formatted JSON.
    /// </summary>
    /// <typeparam name="T">The result type returned by the API operation.</typeparam>
    /// <param name="operation">The operation to execute.</param>
    protected async Task PrintAsync<T>(Task<T> operation)
    {
      // Keep action methods compact by centralizing the await and JSON output.
      T result = await operation.ConfigureAwait(false);
      Ui.PrintJson(result);
    }
  }
}
