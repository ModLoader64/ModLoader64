using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modloader2.Common;

/// <summary>
/// Implementation of all other loggers
/// </summary>
public static class LoggerImpl {
    /// <summary>
    /// Output an Error message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Error(string message) {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Output a Warning message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Warning(string message) {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Output an Info message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Info(string message) {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Output a Debug message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Debug(string message) {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    /// <summary>
    /// Output a message with the defined xolor
    /// </summary>
    /// <param name="message">The message to output</param>
    /// <param name="color">The color to use</param>
    public static void LogColor(string message, ConsoleColor color) {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
