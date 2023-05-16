using Modloader64.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modloader64.Modloader64Internal;

/// <summary>
/// Singleton for internal Modloader logging
/// </summary>
public static class Logger {

    /// <summary>
    /// Output an Error message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Error(string message) {
        LoggerImpl.Error($"[Modloader] {message}");
    }

    /// <summary>
    /// Output a Warning message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Warning(string message) {
        LoggerImpl.Warning($"[Modloader] {message}");
    }

    /// <summary>
    /// Output an Info message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Info(string message) {
        LoggerImpl.Info($"[Modloader] {message}");
    }

    /// <summary>
    /// Output a Debug message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Debug(string message) {
        LoggerImpl.Debug($"[Modloader] {message}");
    }
}


