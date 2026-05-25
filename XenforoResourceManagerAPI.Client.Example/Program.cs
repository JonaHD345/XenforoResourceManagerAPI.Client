using System;
using System.Text;
using System.Threading.Tasks;
using XenforoResourceManagerAPI.Client;

namespace XenforoResourceManagerAPI.Client.Example
{
  /// <summary>
  /// Contains the entry point for the interactive example application.
  /// </summary>
  internal static class Program
  {
    /// <summary>
    /// Creates the shared client and starts the console example.
    /// </summary>
    private static async Task Main()
    {
      // Use UTF-8 so API data and console labels are printed consistently.
      Console.OutputEncoding = Encoding.UTF8;

      // Wire the small example components around one API client instance.
      using XenforoResourceManagerApiClient client = new XenforoResourceManagerApiClient();
      ConsoleUi ui = new ConsoleUi();
      XenforoResourceManagerExampleApplication application = new XenforoResourceManagerExampleApplication(client, ui);

      await application.RunAsync().ConfigureAwait(false);
    }
  }
}
