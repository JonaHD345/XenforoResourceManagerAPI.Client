using System;
using Newtonsoft.Json;
using XenforoResourceManagerAPI.Client;

namespace XenforoResourceManagerAPI.Client.Example
{
  /// <summary>
  /// Provides shared console input, output, and formatting helpers.
  /// </summary>
  internal sealed class ConsoleUi
  {
    private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
    {
      NullValueHandling = NullValueHandling.Include
    };

    /// <summary>
    /// Reads a non-empty string value from the console.
    /// </summary>
    /// <param name="prompt">The prompt shown before reading the value.</param>
    /// <returns>The entered value without surrounding whitespace.</returns>
    public string ReadRequired(string prompt)
    {
      while (true)
      {
        // Keep asking until the user provides content because API validation requires it.
        Console.Write($"{prompt}: ");
        string? value = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(value))
        {
          return value.Trim();
        }

        WriteError("A value is required.");
      }
    }

    /// <summary>
    /// Reads a positive integer value from the console.
    /// </summary>
    /// <param name="prompt">The prompt shown before reading the value.</param>
    /// <returns>The entered positive integer.</returns>
    public int ReadPositiveInt(string prompt)
    {
      while (true)
      {
        // Re-prompt until a valid positive id is entered for API endpoint calls.
        Console.Write($"{prompt}: ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int value) && value > 0)
        {
          return value;
        }

        WriteError("Enter a positive number.");
      }
    }

    /// <summary>
    /// Reads an optional positive integer value from the console.
    /// </summary>
    /// <param name="prompt">The prompt shown before reading the value.</param>
    /// <returns>The entered positive integer, or null when the input is empty.</returns>
    public int? ReadOptionalPositiveInt(string prompt)
    {
      while (true)
      {
        // Empty input is valid for optional filters such as category and page.
        Console.Write($"{prompt}: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
          return null;
        }

        if (int.TryParse(input, out int value) && value > 0)
        {
          return value;
        }

        WriteError("Enter a positive number or leave the value empty.");
      }
    }

    /// <summary>
    /// Writes an object as indented JSON.
    /// </summary>
    /// <param name="value">The value to serialize and print.</param>
    public void PrintJson(object? value)
    {
      // JSON output keeps the example useful even when response models contain many fields.
      Console.WriteLine(JsonConvert.SerializeObject(value, Formatting.Indented, JsonSettings));
    }

    /// <summary>
    /// Waits for the user before returning to the action menu.
    /// </summary>
    public void WaitForMenu()
    {
      // Pause after every action so users can read the response before the menu redraws.
      Console.WriteLine();
      Console.WriteLine("Press Enter to return to the action menu.");
      Console.ReadLine();
    }

    /// <summary>
    /// Writes the example application banner.
    /// </summary>
    public void WriteBanner()
    {
      // Start from a clean console before menu selection begins.
      ClearScreen();
      WriteAccent("XenForo Resource Manager API Console");
      Console.WriteLine("Choose any public API action by number.");
      Console.WriteLine();
    }

    /// <summary>
    /// Writes a section header.
    /// </summary>
    /// <param name="title">The header title.</param>
    public void WriteHeader(string title)
    {
      // Underline headers to make action output easy to scan.
      WriteAccent(title);
      Console.WriteLine(new string('-', title.Length));
    }

    /// <summary>
    /// Writes accent-colored text.
    /// </summary>
    /// <param name="text">The text to write.</param>
    public void WriteAccent(string text)
    {
      // Use cyan for neutral headings and menu groups.
      WriteColored(text, ConsoleColor.Cyan);
    }

    /// <summary>
    /// Writes success-colored text.
    /// </summary>
    /// <param name="text">The text to write.</param>
    public void WriteSuccess(string text)
    {
      // Use green for completed workflows.
      WriteColored(text, ConsoleColor.Green);
    }

    /// <summary>
    /// Writes error-colored text.
    /// </summary>
    /// <param name="text">The text to write.</param>
    public void WriteError(string text)
    {
      // Use red for errors that prevented the selected action from completing.
      WriteColored(text, ConsoleColor.Red);
    }

    /// <summary>
    /// Writes detailed API exception information.
    /// </summary>
    /// <param name="exception">The exception to print.</param>
    public void WriteApiException(XenforoResourceManagerApiException exception)
    {
      // Include status and raw response details when the transport captured them.
      WriteError(exception.Message);

      if (exception.StatusCode.HasValue)
      {
        Console.WriteLine($"HTTP status: {(int)exception.StatusCode.Value} {exception.StatusCode.Value}");
      }

      if (!string.IsNullOrWhiteSpace(exception.ResponseContent))
      {
        Console.WriteLine();
        Console.WriteLine(exception.ResponseContent);
      }
    }

    /// <summary>
    /// Clears the console when output is interactive.
    /// </summary>
    public void ClearScreen()
    {
      // Avoid clearing redirected output so logs remain complete.
      if (!Console.IsOutputRedirected)
      {
        Console.Clear();
      }
    }

    /// <summary>
    /// Writes colored text while preserving the previous console color.
    /// </summary>
    /// <param name="text">The text to write.</param>
    /// <param name="color">The foreground color to use.</param>
    private static void WriteColored(string text, ConsoleColor color)
    {
      // Redirected output should stay plain text without terminal color state changes.
      if (Console.IsOutputRedirected)
      {
        Console.WriteLine(text);
        return;
      }

      ConsoleColor previousColor = Console.ForegroundColor;
      Console.ForegroundColor = color;
      Console.WriteLine(text);
      Console.ForegroundColor = previousColor;
    }
  }
}
