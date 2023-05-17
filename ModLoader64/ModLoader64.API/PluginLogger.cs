using ModLoader64.Common;

namespace ModLoader64.API;

// TODO: make the API aware of the plugin name

/// <summary>
/// Singleton for plugin logging
/// </summary>
public static class PluginLogger
{
    /// <summary>
    /// Output an error message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Error(string message)
    {
        LoggerImpl.Error($"[PLUGIN] {message}");
    }

    /// <summary>
    /// Output a warning message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Warning(string message)
    {
        LoggerImpl.Warning($"[PLUGIN] {message}");
    }

    /// <summary>
    /// Output an info message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Info(string message)
    {
        LoggerImpl.Info($"[PLUGIN] {message}");
    }

    /// <summary>
    /// Output a debug message
    /// </summary>
    /// <param name="message">The message to output</param>
    public static void Debug(string message)
    {
        LoggerImpl.Debug($"[PLUGIN] {message}");
    }
}


