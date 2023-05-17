using System.Runtime.InteropServices;

namespace ModLoader64;

using Core;
using Mupen64plus;
using Logger = Core.Logger;

/// <summary>
/// Main Modloader64 class
/// </summary>
public class Modloader64 {
    public static bool CoreWasInitialized = false;
    public static bool RomWasLoaded = false;

    private static IntPtr RomPtr = IntPtr.Zero;

    /// <summary>
    /// Initializes Mupen64plus
    /// </summary>
    /// <returns>Returns true if success, otherwise false</returns>
    private static unsafe bool Initialize() {
        if (Boot.InitializeCoreStartup() == false) {
            Logger.Error("InitializeCoreStartup failed!");
            return false;
        }

        CoreWasInitialized = true;

        if (Boot.InitializeConfig() == false) {
            Logger.Error("InitializeConfig failed!");
            return false;
        }

        if (Boot.InitializeROM(ref RomPtr) == false) {
            Logger.Error("InitializeROM failed!");
            return false;
        }

        RomWasLoaded = true;

        // Handle cheat codes

        Boot.InitializePlugins();

        return true;
    }

    /// <summary>
    /// Entrypoint of Modloader64
    /// </summary>
    /// <param name="args">parameters passed to the program</param>
    /// <returns>Exit code</returns>
    public static int Main(string[] args) {
        int exit = 0;
        Logger.Info("Hello, world!");
        if (!Initialize()) {
            exit = 1;
        }

        Boot.StartGo();


        Boot.ShutdownPlugins();
        if (RomPtr != IntPtr.Zero) {
            Marshal.FreeHGlobal(RomPtr);
        }

        if (CoreWasInitialized) {
            if (RomWasLoaded) {
                Frontend.CoreDoCommand(M64Command.M64CMD_ROM_CLOSE, 0, IntPtr.Zero);
            }
            Frontend.CoreShutdown();
        }

        return exit;
    }
}

