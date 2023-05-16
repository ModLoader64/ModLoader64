using Modloader64.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Modloader64.Mupen64plus;

/// <summary>
/// Enumeration used for message levels
/// </summary>
public enum M64Message
{
    M64MSG_ERROR = 1,
    M64MSG_WARNING,
    M64MSG_INFO,
    M64MSG_STATUS,
    M64MSG_VERBOSE
}

/// <summary>
/// Singleton for Mupen64plus logging
/// </summary>
public static class Logger
{
    /// <summary>
    /// Helper string to operate with M64Message
    /// </summary>
    public static readonly string[] M64MessageString = {
        "Unknown",
        "Error",
        "Warning",
        "Info",
        "Status",
        "Verbose"
    };

    /// <summary>
    /// Output a message (from Mupen64plus)
    /// </summary>
    /// <param name="message"></param>
    public static void Log(M64Message level, string context, string message)
    {
        string output = $"[{context}] {M64MessageString[(int)level]}: {message}";
        switch (level)
        {
            case M64Message.M64MSG_ERROR:
                LoggerImpl.Error(output);
                break;
            case M64Message.M64MSG_WARNING:
                LoggerImpl.Warning(output);
                break;
            case M64Message.M64MSG_INFO:
                LoggerImpl.Info(output);
                break;
            case M64Message.M64MSG_STATUS:
                LoggerImpl.LogColor(output, ConsoleColor.Green);
                break;
            case M64Message.M64MSG_VERBOSE:
                LoggerImpl.Debug(output);
                break;
        }
    }
}

