using System.Runtime.InteropServices;
using Modloader2.Modloader2Internal;
using Modloader2.Mupen64plus;
using Logger = Modloader2.Modloader2Internal.Logger;

namespace Modloader2;

/// <summary>
/// Main Modloader2 class
/// </summary>
public class Modloader2 {
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
    /// Entrypoint of Modloader2
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

